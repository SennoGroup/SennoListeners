using System.Collections.Generic;
using DataListeners.Model;

namespace DataListeners.Abstraction
{
    /// <summary>
    /// Interface for communication with data storage
    /// </summary>
    public interface IDataService
    {
        DataExtractingQueueItem GetNextFromQueue();
        void SaveRawData(IEnumerable<RawDataItem> data);
        void AddItemToQueue(RawDataResponse parsedRawData);
        void Complete(DataExtractingQueueItem queueItem);
        void Commit();
        void Acquire(DataExtractingQueueItem queueItem);
    }
}
