using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Medium.Services
{
    public class CustomHttpClient : IHttpClient
    {
        public async Task<HttpResponseMessage> PostAsync(string url, object data,
            IDictionary<string, object> headers = null)
        {
            using(var httpClient = new HttpClient())
            {
                foreach(var header in httpClient.DefaultRequestHeaders)
                {
                    httpClient.DefaultRequestHeaders.Remove(header.Key);
                }
                foreach(var header in headers ?? new Dictionary<string, object>())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, new List<string>{header.Value.ToString()});
                }

                var payload = GetJsonContent(data);
                var response = await httpClient.PostAsync(url, payload);

                return response;
            }
        }

        private static StringContent GetJsonContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}