using System.Collections.Generic;

namespace Medium.Domain
{
    public interface IWebhookTriggerValidator<in TRequest> where TRequest : IRequest
    {
         bool Validate(TRequest request, IDictionary<string,Rule> rules);
    }
}