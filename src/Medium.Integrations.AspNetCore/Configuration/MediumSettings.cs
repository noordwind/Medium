using System.Collections.Generic;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class MediumSettings
    {
        public IEnumerable<WebhookModel> Webhooks { get; set; }
    }
}