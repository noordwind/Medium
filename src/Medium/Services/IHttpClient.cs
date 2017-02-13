using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Medium.Services
{
    public interface IHttpClient
    {
         Task<HttpResponseMessage> PostAsync(string url, object data, 
            IDictionary<string, object> headers = null);
    }
}