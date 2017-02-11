using System;
using System.Collections.Generic;
using Medium.Domain;

namespace Medium.Services
{
    public class WebhookTriggerValidatorResolver : IWebhookTriggerValidatorResolver
    {
        private readonly IDictionary<string, Func<IRequest, object, bool>> _validators = new Dictionary<string, Func<IRequest, object, bool>>();
        private readonly IDictionary<string, Type> _requestTypes  = new Dictionary<string, Type>();
        private readonly IDictionary<string, Type> _rulesTypes  = new Dictionary<string, Type>();

        public void Register<TRequest,TRules>(Func<IRequest, object, bool> validator, string type = null)
        {
            if(validator == null)
            {
                throw new ArgumentNullException(nameof(validator), "Validator can not be null.");
            }

            var key = typeof(TRequest).Name.ToLowerInvariant().Replace("request", string.Empty);
            if(!string.IsNullOrWhiteSpace(type))
            {
                key = type.ToLowerInvariant();
            }

            _validators[key] = validator;
            _requestTypes[key] = typeof(TRequest);
            _rulesTypes[key] = typeof(TRules);
        }

        public bool Validate(string type, IRequest request, object rules)
        {
            if(string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"Validator type can not be empty.", nameof(type));
            }
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request can not be null.");
            }
            if(rules == null)
            {
                throw new ArgumentNullException(nameof(rules), "Rules can not be null.");
            }

            var key = type.ToLowerInvariant();
            if(!_validators.ContainsKey(key))
            {
                throw new InvalidOperationException($"Validator for '{type}' was not found.");
            }

            return _validators[key](request, rules);
        }

        public Type GetRequestType(string type)
        {
            if(string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"Validator type can not be empty.", nameof(type));
            }

            var key = type.ToLowerInvariant();
            if(!_validators.ContainsKey(key))
            {
                throw new InvalidOperationException($"Validator for '{type}' was not found.");
            }

            return _requestTypes[key];
        }

        public Type GetRulesType(string type)
        {
            if(string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"Validator type can not be empty.", nameof(type));
            }

            var key = type.ToLowerInvariant();
            if(!_validators.ContainsKey(key))
            {
                throw new InvalidOperationException($"Validator for '{type}' was not found.");
            }

            return _rulesTypes[key];
        }
    }
}