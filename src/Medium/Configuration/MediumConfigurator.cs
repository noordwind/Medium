using System;
using Medium.Domain;
using Medium.Providers;
using Medium.Repositories;
using Medium.Services;

namespace Medium.Configuration
{
    public class MediumConfigurator : IMediumConfigurator
    {
        private IServiceContainer _container;
        private IWebhookTriggerValidatorResolver _resolver;
        private MediumConfiguration _configuration = new MediumConfiguration();
        
        public MediumConfigurator(IServiceContainer container, IWebhookTriggerValidatorResolver resolver)
        {
            _container = container;
            _resolver = resolver;
            _configuration.Resolver = resolver;
            _container.RegisterSingleton<IMediumConfigurator>(this);
        }

        public IMediumConfigurator AddProvider(IProvider provider)
        {
            provider.Register(_resolver);
            _configuration.AddProvider(provider);

            return this;
        }

        public IMediumConfigurator SetRepository(IWebhookRepository repository)
        {
            _container.RegisterTransient<IWebhookRepository>(repository);
            _configuration.Repository = repository;

            return this;
        }

        public IMediumConfigurator AddWebhook(Webhook webhook)
        {
            _configuration.AddWebhook(webhook);

            return this;
        }

        public MediumConfiguration Configure()
        {
            return _configuration;
        }
    }
}