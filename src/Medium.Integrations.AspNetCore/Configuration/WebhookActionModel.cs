using System.Collections.Generic;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class WebhookActionModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public object Request { get; set; }
        public IDictionary<string, object> Headers  { get; set; }
        public bool Enabled { get; set; }        
    }
}