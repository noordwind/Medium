using Medium.Domain;
using Medium.Services;

namespace Medium.Providers
{
    public class Provider<TRequest> : IProvider where TRequest : IRequest
    {
        public void Register(IWebhookTriggerValidatorResolver resolver)
        {
            resolver.Register<TRequest>((request,rules)  => new RequestValidator<TRequest>().Validate((TRequest)request, rules));
        }
    }
}