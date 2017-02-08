using System;
using Medium.Domain;

namespace Medium.Integrations.MyGet
{
    public class MyGetPackageAddedValidator : IValidator<MyGetPackageAddedRequest, MyGetPackageAddedRules>
    {
        public bool Validate(MyGetPackageAddedRequest request, MyGetPackageAddedRules rules)
        {
            throw new NotImplementedException();
        }
    }
}