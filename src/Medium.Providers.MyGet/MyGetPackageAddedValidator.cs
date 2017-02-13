using Medium.Domain;

namespace Medium.Providers.MyGet
{
    public class MyGetPackageAddedValidator : IWebhookTriggerValidator<MyGetPackageAddedRequest, MyGetPackageAddedRules>
    {
        public bool Validate(MyGetPackageAddedRequest request, MyGetPackageAddedRules rules)
        {
            if(rules == null)
            {
                return true;
            }

            return ValidatePackageIdentifier(rules, request.Payload.PackageIdentifier);
        }

        private bool ValidatePackageIdentifier(MyGetPackageAddedRules rules, string identifier)
        {
            if(string.IsNullOrWhiteSpace(identifier))
            {
                return true;
            }
            if(rules.PackageIdentifier == null)
            {
                return true;
            }
            if(rules.PackageIdentifier.Comparison == Comparison.Equals)
            {
                return rules.PackageIdentifier.Value == identifier;
            }

            return false;
        }
    }
}