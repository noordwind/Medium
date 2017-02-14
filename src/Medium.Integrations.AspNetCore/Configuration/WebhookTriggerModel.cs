using System.Collections.Generic;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class WebhookTriggerModel
    {
        public string Name { get; set; }
        public bool Inactive { get; set; }   
        public string Type { get; set; }
        public object Rules { get; set; }
        public IEnumerable<string> Actions { get; set; }
        public IEnumerable<string> Requesters { get; set; }
    }
}