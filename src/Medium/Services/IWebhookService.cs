using System.Collections.Generic;
using System.Threading.Tasks;
using Medium.Domain;

namespace Medium.Services
{
    public interface IWebhookService
    {
         Task<IEnumerable<Webhook>> GetAllAsync();
         Task ExecuteAsync(string endpoint, string trigger, object request, string token = null);
    }
}