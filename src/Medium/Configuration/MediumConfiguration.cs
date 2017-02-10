using System.Collections.Generic;
using Medium.Domain;
using Medium.Providers;
using Medium.Repositories;
using Medium.Services;

namespace Medium.Configuration
{
    public class MediumConfiguration
    {
        private readonly ISet<IProvider> _providers = new HashSet<IProvider>();
        private readonly ISet<Webhook> _webhooks = new HashSet<Webhook>();

        public IWebhookTriggerValidatorResolver Resolver { get; internal set; }
        public IEnumerable<IProvider> Providers => _providers;
        public IEnumerable<Webhook> Webhooks => _webhooks;
        public IWebhookRepository Repository { get; internal set; }

        internal void AddProvider(IProvider provider)
        {
            _providers.Add(provider);
        }

        internal void AddWebhook(Webhook webhook)
        {
            _webhooks.Add(webhook);
        }
    }
}