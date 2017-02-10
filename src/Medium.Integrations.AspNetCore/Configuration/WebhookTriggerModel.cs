using Medium.Domain;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class WebhookTriggerModel
    {
        public string Name { get; set; }
        public string Provider { get; set; }
        public string Type { get; set; }
        public object Rules { get; set; }
        public bool Enabled { get; set; }        
    }
}