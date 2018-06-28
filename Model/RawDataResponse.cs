using System.Collections.Generic;

namespace DataListeners.Model
{
    /// <summary>
    /// Batch response from data listener
    /// </summary>
    public class RawDataResponse
    {
        public List<RawDataItem> Data { get; set; }

        //other properties omitted for brevity
    }
}
