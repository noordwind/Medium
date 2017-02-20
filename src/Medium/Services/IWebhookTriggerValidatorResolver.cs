using System;
using System.Collections.Generic;
using Medium.Domain;

namespace Medium.Services
{
    public interface IWebhookTriggerValidatorResolver
    {
        void Register<TRequest>(Func<IRequest, IDictionary<string, Rule>, bool> validator, string type = null);
        bool Validate(string type, IRequest request, IDictionary<string, Rule> rules);
        Type GetRequestType(string type);
    }
}