using Medium.Domain;
using Medium.Persistence;

namespace Medium.Configuration
{
    public class MediumConfigurator : IMediumConfigurator
    {
        private IServiceContainer _container;
        private MediumConfiguration _configuration = new MediumConfiguration();
        
        public MediumConfigurator(IServiceContainer container)
        {
            _container = container;
            _container.RegisterSingleton<IMediumConfigurator>(this);
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