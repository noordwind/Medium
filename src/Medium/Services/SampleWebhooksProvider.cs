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
                                       new  
                                       {
                                            branch = "develop",
                                            token = "secret"
                                       },
                                       new MyGetPackageAddedRules());

            yield return CreateWebhook("myget-dev-feed-package-added", "myget", 
                                       "build-dev-services-via-travis", "http://travis-ci.com",
                                       new  
                                       {
                                            branch = "master",
                                            token = "secret"
                                       },
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
            webhook.AddAction(action);

            return webhook;
        }
    }
}