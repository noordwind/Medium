using System.Collections.Generic;
using Medium.Domain;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class WebhookTriggerModel
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }   
        public string Type { get; set; }
        public object Rules { get; set; }
        public IEnumerable<string> Requesters { get; set; }
    }
}