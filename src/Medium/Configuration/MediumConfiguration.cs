using System.Collections.Generic;
using Medium.Domain;
using Medium.Persistence;

namespace Medium.Configuration
{
    public class MediumConfiguration
    {
        private readonly ISet<Webhook> _webhooks = new HashSet<Webhook>();
        public IEnumerable<Webhook> Webhooks => _webhooks;
        public IWebhookRepository Repository { get; internal set; }

        internal void AddWebhook(Webhook webhook)
        {
            _webhooks.Add(webhook);
        }
    }
}