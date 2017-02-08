using System.Collections.Generic;
using Medium.Domain;

namespace Medium.Services
{
    public interface ISampleWebhooksProvider
    {
         IEnumerable<Webhook> GetAll();
    }
}