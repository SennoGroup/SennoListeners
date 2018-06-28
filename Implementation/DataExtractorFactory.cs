using System;
using DataListeners.Abstraction;
using DataListeners.Model;

namespace DataListeners.Implementation
{
    /// <summary>
    /// Factory for Data Listeners instantiating
    /// </summary>
    public static class DataExtractorFactory
    {
        public static IDataExtractor GetDataExtractor(IServiceProvider serviceProvider, DataProvider dataProvider)
        {
            if (!(serviceProvider.GetService(Type.GetType(dataProvider.DataExtractorType)) is IDataExtractor plugin))
                throw new NotSupportedException();

            return plugin;
        }
    }
}
