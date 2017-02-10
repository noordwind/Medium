using System.Collections.Generic;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class Medium
    {
        public IEnumerable<WebhookModel> Webhooks { get; set; }
    }
}