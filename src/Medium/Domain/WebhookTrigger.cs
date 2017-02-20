using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Medium.Domain
{
    public class WebhookTrigger
    {
        private static readonly Regex NameRegex = new Regex("([a-zA-Z1-9 _\\-])\\w+", RegexOptions.Compiled);
        private ISet<string> _actions = new HashSet<string>();  
        private ISet<string> _requesters = new HashSet<string>();  
        public string Name { get; protected set; }
        public IDictionary<string, IDictionary<string, Rule>> Rules { get; protected set; } = new Dictionary<string, IDictionary<string, Rule>>();
        public bool Inactive { get; protected set; }
        public IDictionary<string, IEnumerable<string>> RulesActions  { get; protected set; } = new Dictionary<string, IEnumerable<string>>();

        public IEnumerable<string> Actions 
        {
            get { return _actions; }
            set 
            {
                _actions = new HashSet<string>(value);
            }
        }

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

        protected WebhookTrigger(string name)
        {
            SetName(name);
            Activate();
        }

        public static WebhookTrigger Create(string name) 
            {
                var trigger = new WebhookTrigger(name);

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

            Name = name.Trim().Replace(" ", "-").ToLowerInvariant();
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
            _requesters.Add(host);
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

        public void AddAction(string action)
        {
            if(string.IsNullOrWhiteSpace(action))
            {
                throw new ArgumentException("Webhook trigger action can not be empty.", nameof(action));
            }

            var actionName = action.Trim().Replace(" ", "-").ToLowerInvariant();
            var existingAction = _actions.SingleOrDefault(x => x.Equals(actionName));
            if(existingAction != null)
            {
                throw new ArgumentException($"Webhook trigger action: '{action}' already exists.", nameof(action));
            }
            _actions.Add(actionName);
        }

        public void RemoveActions(string action)
        {
            if(string.IsNullOrWhiteSpace(action))
            {
                throw new ArgumentException("Webhook trigger action can not be empty.", nameof(action));
            }

            var actionName = action.Trim().Replace(" ", "-").ToLowerInvariant();
            var existingAction = _actions.SingleOrDefault(x => x.Equals(actionName));
            if(existingAction == null)
            {
                return;
            }
            _actions.Remove(existingAction);
        }

        public void Activate()
        {
            Inactive = false;
        }

        public void Deactivate()
        {
            Inactive = true;
        }
    }
}