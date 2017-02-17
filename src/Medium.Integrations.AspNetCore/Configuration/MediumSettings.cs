using System.Collections.Generic;
using Medium.Configuration.Models;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class MediumSettings
    {
        public IEnumerable<WebhookModel> Webhooks { get; set; }
    }
}