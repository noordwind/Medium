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
        public string Type { get; protected set; }
        public object Rules { get; protected set; }
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

        public WebhookTrigger(string name)
        {
            SetName(name);
            Enable();
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
            Type = rules.GetType().Name.ToLowerInvariant().Replace("rules", string.Empty);
        }

        public void ClearRules()
        {
            Rules = null;
            Type = null;
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
    }
}