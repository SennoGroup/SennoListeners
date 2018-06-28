using System;
using System.Threading.Tasks;
using DataListeners.Abstraction;
using DataListeners.Model;
using Microsoft.Extensions.Configuration;

namespace DataListeners.Implementation
{
    /// <summary>
    /// Service for processing data extraction queue
    /// </summary>
    public class DataExtractingService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;
        private static readonly object _monitor = new object();

        public DataExtractingService(IServiceProvider serviceProvider, IConfiguration configuration, IDataService dataService)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _dataService = dataService;
        }

        /// <summary>
        /// Main method for queue processing
        /// </summary>
        public async Task ExtractNextBatchAsync()
        {
            if (!GetNext(out var queueItem)) return;

            var dataExtractor = DataExtractorFactory.GetDataExtractor(_serviceProvider, queueItem.DataProvider);

            var parsedRawData = await dataExtractor.GetRawData(queueItem);
            if (parsedRawData != null)
            {
                _dataService.SaveRawData(parsedRawData.Data);
                _dataService.AddItemToQueue(parsedRawData);
                _dataService.Complete(queueItem);
                _dataService.Commit();
            }
        }

        private bool GetNext(out DataExtractingQueueItem queueItem)
        {
            lock (_monitor)
            {
                queueItem = _dataService.GetNextFromQueue();
                if (queueItem == null) return false;
                _dataService.Acquire(queueItem);
            }
            return true;
        }
    }
}
