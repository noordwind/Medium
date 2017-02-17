using System.Collections.Generic;
using Medium.Domain;

namespace Medium.Configuration.Models
{
    public class WebhookActionModel
    {
        public string Name { get; set; }
        public string Codename { get; set; }
        public bool Inactive { get; set; }        
        public string Url { get; set; }
        public object Request { get; set; }
        public IDictionary<string, object> Headers  { get; set; }

        public static WebhookAction MapToWebhookAction(WebhookActionModel model)
        {
            var action = new WebhookAction(model.Name, model.Url, model.Request);
            action.SetCodename(model.Codename);
            if(model.Inactive)
            {
                action.Deactivate();
            }
            foreach (var header in model.Headers ?? new Dictionary<string, object>())
            {
                action.Headers[header.Key] = header.Value;
            }

            return action;
        }
    }
}