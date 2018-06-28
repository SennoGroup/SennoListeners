using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DataListeners.Implementation
{
    /// <summary>
    /// Base class for Data Listeners
    /// </summary>
    public abstract class BaseDataExtractor
    {
        protected const string GZIP_FLAG = "gzip";
        private const string JSON_MEDIA_TYPE = "application/json";

        protected HttpClient CreateClient(string apiUrl)
        {
            var client = new HttpClient {BaseAddress = new Uri(apiUrl)};
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JSON_MEDIA_TYPE));
            return client;
        }

        protected async Task<TResult> SendRequestAsync<TResult>(string url, HttpMethod method, Action<HttpClient, HttpRequestMessage> setHeaders = null)
        {
            using (var client = CreateClient(url))
            {
                var request = new HttpRequestMessage(method, url);
                setHeaders?.Invoke(client, request);

                var httpResponseMessage = await client.SendAsync(request);
                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                        throw new UnauthorizedAccessException();
                }

                var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.Content.Headers.ContentEncoding.Contains(GZIP_FLAG))
                {
                    using (var decompress = new GZipStream(httpResponseMessage.Content.ReadAsStreamAsync().Result, CompressionMode.Decompress))
                    using (var sr = new StreamReader(decompress))
                    {
                        content = sr.ReadToEnd();
                    }
                }

                var result = JsonConvert.DeserializeObject<TResult>(content, new IsoDateTimeConverter
                    {DateTimeFormat = DateTimeFormat, Culture = System.Globalization.CultureInfo.InvariantCulture});
                return result;
            }
        }

        /// <summary>
        /// ISO 8601 by default
        /// </summary>
        protected virtual string DateTimeFormat => "yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz";
    }
}
