using System;
using Medium.Domain;

namespace Medium.Services
{
    public interface IWebhookTriggerValidatorResolver
    {
        void Register<TRequest>(Func<IRequest, IRules, bool> validator, string type = null);
        bool Validate(string type, IRequest request, IRules rules);
        Type GetRequestType(string type);
    }
}