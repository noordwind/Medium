namespace Medium.Domain
{
    public interface IWebhookTriggerValidator<in TRequest, in TRules> 
        where TRequest : IRequest where TRules : IRules
    {
         bool Validate(TRequest request, TRules rules);
    }
}