using System.Collections.Generic;
using Medium.Domain;

namespace Medium.Services
{
    public interface IRequestValidator
    {
         bool Validate(dynamic request, IDictionary<string, Rule> rules);
    }
}