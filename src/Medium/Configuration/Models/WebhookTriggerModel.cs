using System;
using System.Collections.Generic;
using System.Linq;
using Medium.Domain;

namespace Medium.Configuration.Models
{
    public class WebhookTriggerModel
    {
        public string Name { get; set; }
        public bool Inactive { get; set; }   
        public IDictionary<string, IDictionary<string, RuleModel>> Rules { get; set; }
        public IDictionary<string, IEnumerable<string>> RulesActions  { get;  set; }
        public IEnumerable<string> Actions { get; set; }
        public IEnumerable<string> Requesters { get; set; }

        public static WebhookTrigger MapToWebhookTrigger(WebhookTriggerModel model)
        {
            var trigger = WebhookTrigger.Create(model.Name);
            if(model.Inactive)
            {
                trigger.Deactivate();
            }
            foreach (var action in model.Actions ?? Enumerable.Empty<string>())
            {
                trigger.AddAction(action);
            }
            foreach (var requester in model.Requesters ?? Enumerable.Empty<string>())
            {
                trigger.AddRequester(requester);
            }
            foreach (var rule in model.Rules ?? new Dictionary<string, IDictionary<string, RuleModel>>())
            {
                trigger.Rules[rule.Key] = rule.Value.ToDictionary(x => x.Key, x => new Rule
                {
                    Comparison = (Comparison)Enum.Parse(typeof(Comparison), x.Value.Comparison, true),
                    Value = x.Value.Value
                });
            }
            foreach (var ruleActions in model.RulesActions ?? new Dictionary<string, IEnumerable<string>>())
            {
                trigger.RulesActions[ruleActions.Key] = ruleActions.Value;
            }

            return trigger;
        }
    }
}