using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medium.Domain;
using Medium.Integrations.MyGet;
using Medium.Repositories;
using Newtonsoft.Json;

namespace Medium.Services

{
    public class WebhookService : IWebhookService
    {
        private readonly IWebhookRepository _webhookRepository;

        public WebhookService(IWebhookRepository webhookRepository)
        {
            _webhookRepository = webhookRepository;
        }

        public async Task<IEnumerable<Webhook>> GetAllAsync() 
            => await _webhookRepository.GetAllAsync();

        public async Task ExecuteAsync(string endpoint, string trigger, object request, string token = null)
        {
            var webhook = await _webhookRepository.GetAsync(endpoint);
            if(webhook == null)
            {
                throw new ArgumentException($"Webhook was not found for endpoint: '{endpoint}'.");
            }
            if(!webhook.Enabled)
            {
                throw new ArgumentException($"Webhook '{webhook.Name}' is not enabled.");
            }
            if(!string.IsNullOrWhiteSpace(webhook.Token) && webhook.Token != token)
            {
                throw new ArgumentException($"Invalid token for webhook '{webhook.Name}'.");
            }
            
            var triggerToValidate = webhook.Triggers.SingleOrDefault(x => x.Name == trigger.ToLowerInvariant());
            if(triggerToValidate == null)
            {
                throw new ArgumentException($"Trigger '{trigger}' was not found for webhook '{webhook.Name}'.");
            }

            var serializedRequest = JsonConvert.SerializeObject(request);
            var deserializedRequest = JsonConvert.DeserializeObject<MyGetPackageAddedRequest>(serializedRequest);
        }

        private void ValidateRequest(object request)
        {
        }
    }
}