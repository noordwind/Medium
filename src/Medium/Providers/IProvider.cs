using Medium.Services;

namespace Medium.Providers
{
    public interface IProvider
    {
         void Register(IWebhookTriggerValidatorResolver resolver);
    }
}