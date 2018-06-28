using System.Threading.Tasks;
using DataListeners.Model;

namespace DataListeners.Abstraction
{
    /// <summary>
    /// Common interface for all data listeners
    /// </summary>
    public interface IDataExtractor
    {
        Task<RawDataResponse> GetRawData(DataExtractingQueueItem queueItem);
    }
}
