using System.Collections.Generic;

namespace Medium.Integrations.AspNetCore.Configuration
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
    }
}