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
            => await ExecuteAsync(endpoint, trigger, JsonConvert.SerializeObject(request), token);

        public async Task ExecuteAsync(string endpoint, string trigger, string serializedRequest, string token = null)
        {
            var webhook = await GetAndValidateWebhookAsync(endpoint, token);
            var webhookTriggerAndRules = GetAndValidateTrigger(webhook, trigger, serializedRequest);
            await ExecuteActionsAsync(webhookTriggerAndRules, webhook);
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

        private Tuple<WebhookTrigger, IDictionary<string, IEnumerable<string>>> GetAndValidateTrigger(Webhook webhook, string triggerName, string serializedRequest)
        {
            var trigger = webhook.Triggers.SingleOrDefault(x => x.Name == triggerName.ToLowerInvariant());
            if(trigger == null)
            {
                throw new ArgumentException($"Trigger '{triggerName}' was not found for webhook '{webhook.Name}'.");
            }

            var requestType = _validatorResolver.GetRequestType(trigger.Type);
            var rulesType = _validatorResolver.GetRulesType(trigger.Type);
            var deserializedRequest = JsonConvert.DeserializeObject(serializedRequest, requestType) as IRequest;
            var rulesActions = new Dictionary<string, IEnumerable<string>>();
            foreach(var rule in trigger.Rules)
            {
                var serializedRules = JsonConvert.SerializeObject(rule.Value);
                var deserializedRules = JsonConvert.DeserializeObject(serializedRules, rulesType);
                var isRuleValid = _validatorResolver.Validate(trigger.Type, deserializedRequest, deserializedRules);
                if(isRuleValid)
                {
                    if(trigger.RulesActions.ContainsKey(rule.Key))
                    {
                        rulesActions[rule.Key] = trigger.RulesActions[rule.Key];
                    }
                    else
                    {
                        rulesActions[rule.Key] = Enumerable.Empty<string>();
                    }
                }
            }

            if(!rulesActions.Any())
            {
                throw new InvalidOperationException($"Trigger '{trigger.Name}' is not valid for webhook '{webhook.Name}'.");
            }

            return new Tuple<WebhookTrigger, IDictionary<string, IEnumerable<string>>>(trigger, rulesActions);
        }

        private async Task ExecuteActionsAsync(Tuple<WebhookTrigger, IDictionary<string, IEnumerable<string>>> triggerAndRulesActions, Webhook webhook)
        {
            var trigger = triggerAndRulesActions.Item1;
            var rulesActions = triggerAndRulesActions.Item2;
            if(trigger.Actions.Any(x => x == "*"))
            {
                await ExecuteAllActionsAsync(webhook);
            }

            var tasks = new List<Task>();
            tasks.AddRange(GetTriggerActions(webhook, trigger.Actions));
            foreach(var ruleActions in rulesActions)
            {
                tasks.AddRange(GetTriggerActions(webhook, ruleActions.Value));
            }
            await Task.WhenAll(tasks);
        }

        private IEnumerable<Task> GetTriggerActions(Webhook webhook, IEnumerable<string> actions)
        {
            if(actions.Any(x => x == "*"))
            {
                foreach(var action in webhook.Actions)
                {
                    if(action.Inactive)
                    {
                        continue;
                    }

                    yield return  _httpClient.PostAsync(action.Url, action.Request, action.Headers);
                }

                yield break;
            }

            foreach(var codename in actions)
            {
                var action = webhook.Actions.Single(x => x.EqualsCodename(codename));
                if(action.Inactive)
                {
                    continue;
                }

                yield return  _httpClient.PostAsync(action.Url, action.Request, action.Headers);
            }
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