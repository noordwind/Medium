using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Medium.Domain
{
    public class WebhookTrigger
    {
        private static readonly Regex NameRegex = new Regex("([a-zA-Z1-9 _\\-])\\w+", RegexOptions.Compiled);
        private ISet<string> _requesters = new HashSet<string>();  
        public string Name { get; protected set; }
        public string Provider { get; protected set; }
        public string Type { get; protected set; }
        public IRules Rules { get; protected set; }
        public bool Enabled { get; protected set; }

        public IEnumerable<string> Requesters 
        {
            get { return _requesters; }
            set 
            {
                _requesters = new HashSet<string>(value);
            }
        }

        protected WebhookTrigger()
        {
        }

        protected WebhookTrigger(string name, string type = null)
        {
            SetName(name);
            Type = type;
            Enable();
        }

        public static WebhookTrigger Create(string name, string type) 
            {
                var trigger = new WebhookTrigger(name, type);

                return trigger;
            }

        public static WebhookTrigger Create<TRequest>(string name) 
            where TRequest : IRequest
            {
                var trigger = new WebhookTrigger(name);
                trigger.SetType<TRequest>();

                return trigger;
            }

        public static WebhookTrigger Create<TRequest, TRules>(string name, TRules rules) 
            where TRequest : IRequest where TRules : IRules
            {
                var trigger = new WebhookTrigger(name);
                trigger.SetRules(rules);
                trigger.SetType<TRequest>();

                return trigger;
            }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Webhook trigger name can not be empty.", nameof(name));
            }
            if (name.Length < 3)
            {
                throw new ArgumentException("Webhook actitriggeron name is too short.", nameof(name));
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("Webhook trigger name is too long.", nameof(name));
            }
            if (!NameRegex.IsMatch(name))
            {
                throw new ArgumentException("Webhook trigger name doesn't match the required criteria.", nameof(name));
            }

            Name = name.Trim().ToLowerInvariant();
        }

        public void SetRules<T>(T rules) where T : IRules
        {
            if(rules == null)
            {
                ClearRules();

                return;
            }
            Rules = rules;
            Provider = rules.Provider;
        }

        public void ClearRules()
        {
            Rules = null;
            Provider = null;
        }

        public void AddRequester(string host)
        {
            if(string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Webhook trigger requester can not be empty.", nameof(host));
            }

            var requester = _requesters.SingleOrDefault(x => x.Equals(host));
            if(requester != null)
            {
                throw new ArgumentException($"Webhook trigger requester: '{host}' already exists.", nameof(host));
            }
            _requesters.Add(requester);
        }

        public void RemoveRequester(string host)
        {
            if(string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Webhook trigger requester can not be empty.", nameof(host));
            }

            var requester = _requesters.SingleOrDefault(x => x.Equals(host));
            if(requester == null)
            {
                return;
            }
            _requesters.Remove(requester);
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        private void SetType<TRequest>()
        {
            Type = typeof(TRequest).Name.ToLowerInvariant().Replace("request", string.Empty);
        }
    }
}