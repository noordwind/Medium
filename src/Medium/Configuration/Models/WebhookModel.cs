using System.Collections.Generic;
using System.Linq;
using Medium.Domain;

namespace Medium.Configuration.Models
{
    public class WebhookModel
    {
        public string Name { get; set; }
        public bool Inactive { get; set; }  
        public string Endpoint { get; set; }
        public string Token { get; set; }
        public object DefaultRequest { get; set; }
        public IDictionary<string, object> DefaultHeaders  { get; set; }
        public IEnumerable<WebhookActionModel> Actions { get; set; }         
        public IEnumerable<WebhookTriggerModel> Triggers { get; set; } 

        public static Webhook MapToWebhook(WebhookModel model)
        {
            var webhook = new Webhook(model.Name, model.Endpoint);
            if(model.Inactive)
            {
                webhook.Deactivate();
            }
            if(!string.IsNullOrWhiteSpace(model.Token))
            {
                webhook.SetToken(model.Token);
            }
            webhook.SetDefaultRequest(model.DefaultRequest);
            foreach (var header in model.DefaultHeaders ?? new Dictionary<string, object>())
            {
                webhook.DefaultHeaders[header.Key] = header.Value;
            }
            foreach(var action in model.Actions ?? Enumerable.Empty<WebhookActionModel>())
            {
                webhook.AddAction(WebhookActionModel.MapToWebhookAction(action));
            }
            foreach(var trigger in model.Triggers ?? Enumerable.Empty<WebhookTriggerModel>())
            {   
                webhook.AddTrigger(WebhookTriggerModel.MapToWebhookTrigger(trigger));
            }

            return webhook;            
        }
    }
}