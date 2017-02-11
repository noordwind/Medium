namespace Medium.Domain
{
    public interface IWebhookTriggerValidator<in TRequest, in TRules> 
        where TRequest : IRequest where TRules : class
    {
         bool Validate(TRequest request, TRules rules);
    }
}