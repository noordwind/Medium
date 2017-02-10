using Medium.Domain;
using Medium.Providers;
using Medium.Repositories;

namespace Medium.Configuration
{
    public interface IMediumConfigurator
    {
        IMediumConfigurator AddProvider(IProvider provider);
        IMediumConfigurator SetRepository(IWebhookRepository repository);
        IMediumConfigurator AddWebhook(Webhook webhook);
        MediumConfiguration Configure();
    }
}