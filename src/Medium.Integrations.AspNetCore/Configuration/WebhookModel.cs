using System.Collections.Generic;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class WebhookModel
    {
        public string Name { get; set; }
        public string Endpoint { get; set; }
        public string Token { get; set; }
        public bool Enabled { get; set; }  
        public IEnumerable<WebhookActionModel> Actions { get; set; }         
        public IEnumerable<WebhookTriggerModel> Triggers { get; set; } 
    }
}