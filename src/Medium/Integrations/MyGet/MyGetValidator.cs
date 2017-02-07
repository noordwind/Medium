using System;
using Medium.Domain;

namespace Medium.Integrations.MyGet
{
    public class MyGetValidator : IValidator<MyGetRequest, MyGetRules>
    {
        public bool Validate(MyGetRequest request, MyGetRules rules)
        {
            throw new NotImplementedException();
        }
    }
}