using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DataListeners.Abstraction;
using DataListeners.Implementation;
using DataListeners.Model;
using Microsoft.Extensions.Configuration;

namespace DataListeners.Integrations.Twitter
{
    public class TwitterDataExtractor : BaseDataExtractor, IDataExtractor
    {
        private readonly IConfiguration _configuration;
        private const string AUTH_KEY = "Bearer";

        protected override string DateTimeFormat => "ddd MMM dd HH:mm:ss +0000 yyyy";

        public TwitterDataExtractor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RawDataResponse> GetRawData(DataExtractingQueueItem queueItem)
        {
            var accessToken = GetAccessToken(queueItem);
            var requestUrl = GetUrl();

            TwitterDataResponse response;
            try
            {
                response = await SendRequestAsync<TwitterDataResponse>(requestUrl, HttpMethod.Get, (client, request) =>
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(AUTH_KEY, accessToken);
                    request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(GZIP_FLAG));
                });
            }
            catch (UnauthorizedAccessException)
            {
                RenewToken(accessToken);
                return await GetRawData(queueItem);
            }

            var result = Map(response);
            return result;
        }

        private RawDataResponse Map(TwitterDataResponse response)
        {
            //omitted for brevity
            throw new NotImplementedException();
        }

        private void RenewToken(string accessToken)
        {
            //omitted for brevity
            throw new NotImplementedException();
        }

        private string GetUrl()
        {
            //omitted for brevity
            throw new NotImplementedException();
        }

        private string GetAccessToken(DataExtractingQueueItem queueItem)
        {
            //omitted for brevity
            throw new NotImplementedException();
        }
    }
}
