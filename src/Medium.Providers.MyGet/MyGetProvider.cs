using Medium.Services;

namespace Medium.Providers.MyGet
{
    public class MyGetProvider : IProvider
    {
        public void Register(IWebhookTriggerValidatorResolver resolver)
        {
            resolver.Register<MyGetPackageAddedRequest,MyGetPackageAddedRules>((request,rules)  => 
                new MyGetPackageAddedValidator().Validate((MyGetPackageAddedRequest)request, (MyGetPackageAddedRules)rules));
        }
    }
}