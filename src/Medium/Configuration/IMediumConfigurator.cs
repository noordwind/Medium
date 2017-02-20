using Medium.Domain;
using Medium.Persistence;

namespace Medium.Configuration
{
    public interface IMediumConfigurator
    {
        IMediumConfigurator SetRepository(IWebhookRepository repository);
        IMediumConfigurator AddWebhook(Webhook webhook);
        MediumConfiguration Configure();
    }
}