using System.Collections.Generic;
using System.Linq;
using Medium.Domain;
using Newtonsoft.Json.Linq;

namespace Medium.Services
{
    public class RequestValidator : IRequestValidator
    {
        private IList<bool> _validatationResults = new List<bool>();

        public bool Validate(dynamic request, IDictionary<string, Rule> rules)
        {
            if(rules == null)
            {
                return true;
            }

            var entries = new JsonParser().Parse((JObject)request);
            foreach(var mappedRule in rules)
            {
                var entry = entries.FirstOrDefault(x => x.Key.ToLowerInvariant() == mappedRule.Key.Replace(".", ":").ToLowerInvariant());
                _validatationResults.Add(entry.Value == mappedRule.Value.Value);
            }
            
            return _validatationResults.All(x => x);
        }
    }
}