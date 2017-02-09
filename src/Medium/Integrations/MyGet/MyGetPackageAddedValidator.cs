using System;
using Medium.Domain;

namespace Medium.Integrations.MyGet
{
    public class MyGetPackageAddedValidator : IWebhookTriggerValidator<MyGetPackageAddedRequest, MyGetPackageAddedRules>
    {
        public bool Validate(MyGetPackageAddedRequest request, MyGetPackageAddedRules rules)
        {
            return true;
        }
    }
}