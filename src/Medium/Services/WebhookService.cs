using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medium.Domain;
using Medium.Repositories;
using Newtonsoft.Json;

namespace Medium.Services

{
    public class WebhookService : IWebhookService
    {
        private readonly IWebhookRepository _webhookRepository;
        private readonly IWebhookTriggerValidatorResolver _validatorResolver;
        private readonly IHttpClient _httpClient;

        public WebhookService(IWebhookRepository webhookRepository, 
            IWebhookTriggerValidatorResolver validatorResolver,
            IHttpClient httpClient)
        {
            _webhookRepository = webhookRepository;
            _validatorResolver = validatorResolver;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Webhook>> GetAllAsync() 
            => await _webhookRepository.GetAllAsync();

        public async Task ExecuteAsync(string endpoint, string trigger, object request, string token = null)
        {
            var webhook = await GetAndValidateWebhookAsync(endpoint, token);
            var webhookTrigger = GetAndValidateTrigger(webhook, trigger, request);
            await ExecuteActionsAsync(webhookTrigger, webhook);
        }

        private async Task<Webhook> GetAndValidateWebhookAsync(string endpoint, string token = null)
        {
            var webhook = await _webhookRepository.GetAsync(endpoint);
            if(webhook == null)
            {
                throw new ArgumentException($"Webhook was not found for endpoint: '{endpoint}'.");
            }
            if(webhook.Inactive)
            {
                throw new ArgumentException($"Webhook '{webhook.Name}' is inactive.");
            }
            if(!string.IsNullOrWhiteSpace(webhook.Token) && webhook.Token != token)
            {
                throw new ArgumentException($"Invalid token for webhook '{webhook.Name}'.");
            }

            return webhook;
        }

        private WebhookTrigger GetAndValidateTrigger(Webhook webhook, string triggerName, object request)
        {
            var trigger = webhook.Triggers.SingleOrDefault(x => x.Name == triggerName.ToLowerInvariant());
            if(trigger == null)
            {
                throw new ArgumentException($"Trigger '{triggerName}' was not found for webhook '{webhook.Name}'.");
            }

            var requestType = _validatorResolver.GetRequestType(trigger.Type);
            var rulesType = _validatorResolver.GetRulesType(trigger.Type);
            var serializedRequest = JsonConvert.SerializeObject(request);
            var deserializedRequest = JsonConvert.DeserializeObject(serializedRequest, requestType) as IRequest;
            var serializedRules = JsonConvert.SerializeObject(trigger.Rules);
            var deserializedRules = JsonConvert.DeserializeObject(serializedRules, rulesType);
            var isTriggerValid = _validatorResolver.Validate(trigger.Type, deserializedRequest, deserializedRules);
            if(!isTriggerValid)
            {
                throw new InvalidOperationException($"Trigger '{trigger.Name}' is not valid for webhook '{webhook.Name}'.");
            }

            return trigger;
        }

        private async Task ExecuteActionsAsync(WebhookTrigger trigger, Webhook webhook)
        {
            if(trigger.Actions.Any(x => x == "*"))
            {
                await ExecuteAllActionsAsync(webhook);

                return;
            }

            var tasks = new List<Task>();
            foreach(var codename in trigger.Actions)
            {
                var action = webhook.Actions.Single(x => x.Codename == codename);
                if(action.Inactive)
                {
                    continue;
                }
                var task = _httpClient.PostAsync(action.Url, action.Request, action.Headers);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }

        private async Task ExecuteAllActionsAsync(Webhook webhook)
        {
            var tasks = new List<Task>();
            foreach(var action in webhook.Actions)
            {
                var task = _httpClient.PostAsync(action.Url, action.Request, action.Headers);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }
    }
}