using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Medium.Services
{
    public class CustomHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public CustomHttpClient()
        {
            _httpClient  = new HttpClient();
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<HttpResponseMessage> PostAsync(string url, object data)
        {
            var payload = GetJsonContent(data);
            var response = await _httpClient.PostAsync(url, payload);

            return response;
        }

        private static StringContent GetJsonContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}