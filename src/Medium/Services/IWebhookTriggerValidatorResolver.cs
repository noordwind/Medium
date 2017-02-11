using System;
using Medium.Domain;

namespace Medium.Services
{
    public interface IWebhookTriggerValidatorResolver
    {
        void Register<TRequest,TRules>(Func<IRequest, object, bool> validator, string type = null);
        bool Validate(string type, IRequest request, object rules);
        Type GetRequestType(string type);
        Type GetRulesType(string type);
    }
}