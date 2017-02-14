using System.Collections.Generic;
using System.Linq;
using Medium.Domain;

namespace Medium.Providers.MyGet
{
    public class MyGetPackageAddedValidator : IWebhookTriggerValidator<MyGetPackageAddedRequest, MyGetPackageAddedRules>
    {
        private IList<bool> _validatationResults = new List<bool>();

        public bool Validate(MyGetPackageAddedRequest request, MyGetPackageAddedRules rules)
        {
            if(rules == null)
            {
                return true;
            }

            _validatationResults.Add(ValidateFeedIdentifier(rules, request.Payload.FeedIdentifier));
            _validatationResults.Add(ValidatePackageIdentifier(rules, request.Payload.PackageIdentifier));

            return _validatationResults.All(x => x);
        }

        private bool ValidateFeedIdentifier(MyGetPackageAddedRules rules, string identifier)
        {
            if(string.IsNullOrWhiteSpace(identifier))
            {
                return true;
            }
            if(rules.FeedIdentifier == null)
            {
                return true;
            }
            if(rules.FeedIdentifier.Comparison == Comparison.Equals)
            {
                return rules.FeedIdentifier.Value == identifier;
            }

            return false;
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