using Medium.Domain;
using System.Collections.Generic;

namespace Medium.Services
{
    public interface IRequestValidator
    {
         bool Validate(dynamic request, IDictionary<string, Rule> rules);
    }
}