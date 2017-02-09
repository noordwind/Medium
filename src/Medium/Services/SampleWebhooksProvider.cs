using System.Collections.Generic;
using Medium.Domain;
using Medium.Integrations.MyGet;

namespace Medium.Services
{
    public class SampleWebhooksProvider : ISampleWebhooksProvider
    {
        public IEnumerable<Webhook> GetAll()
        {
            yield return CreateWebhook("myget-feed-package-added", "myget", 
                                       "build-services-via-travis", "http://travis-ci.com",
                                       new {source_type = "Branch", source_name = "develop" },
                                       new MyGetPackageAddedRules());

            yield return CreateWebhook("myget-dev-feed-package-added", "myget", 
                                       "build-dev-services-via-travis", "http://travis-ci.com",
                                       new {source_type = "Branch", source_name = "master" },
                                       new MyGetPackageAddedRules());
        }

        private Webhook CreateWebhook<TRules>(string name, string triggerName, 
            string actionName, string actionUrl, object actionRequest, 
            TRules rules) where TRules : IRules
        {
            var webhook = new Webhook(name);
            var trigger = WebhookTrigger.Create<MyGetPackageAddedRequest, TRules>(triggerName, rules);
            webhook.AddTrigger(trigger);

            var action = new WebhookAction(actionName, actionUrl, actionRequest);
            action.Headers["content-type"] = "application/json";
            action.Headers["accept"] = "application/json";
            webhook.AddAction(action);

            return webhook;
        }
    }
}