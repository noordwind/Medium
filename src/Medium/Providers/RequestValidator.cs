using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Medium.Domain;

namespace Medium.Providers
{
    public class RequestValidator<TRequest> : IWebhookTriggerValidator<TRequest> where TRequest : IRequest
    {
        private IList<bool> _validatationResults = new List<bool>();

        public bool Validate(TRequest request, IDictionary<string, Rule> rules)
        {
            if(rules == null)
            {
                return true;
            }

            foreach(var keyValue in rules)
            {
                var value = GetPropertyValue(keyValue.Key, request, request.GetType());
                _validatationResults.Add(value.ToString() == keyValue.Value.Value);
            }
            
            return _validatationResults.All(x => x);
        }

        private static object GetPropertyValue(string name, object obj, Type type)
            {
                var parts = name.Split('.').ToList();
                var currentPart = parts[0];
                var typeName = type.Name;
                var property = type.GetTypeInfo()
                                    .GetProperty(currentPart, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property == null) 
                { 
                    return null; 
                }
                if (name.IndexOf(".") > -1)
                {
                    parts.Remove(currentPart);

                    return GetPropertyValue(String.Join(".", parts), property.GetValue(obj, null), property.PropertyType);
                } 
                else
                {
                    return property.GetValue(obj, null).ToString();
                }
            }
    }
}