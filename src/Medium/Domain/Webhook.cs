using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Medium.Domain
{
    public class Webhook
    {
        private static readonly Regex NameRegex = new Regex("([a-zA-Z1-9 _\\-])\\w+", RegexOptions.Compiled);
        private ISet<WebhookAction> _actions = new HashSet<WebhookAction>();  
        private ISet<WebhookTrigger> _triggers = new HashSet<WebhookTrigger>();  

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Endpoint { get; protected set; }
        public string Token { get; protected set; }
        public bool Enabled { get; protected set; }

        public IEnumerable<WebhookAction> Actions 
        {
            get { return _actions; }
            set 
            {
                _actions = new HashSet<WebhookAction>(value);
            }
        }

        public IEnumerable<WebhookTrigger> Triggers 
        {
            get { return _triggers; }
            set 
            {
                _triggers = new HashSet<WebhookTrigger>(value);
            }
        }

        protected Webhook()
        {
        }

        public Webhook(string name)
        {
            Id = Guid.NewGuid();
            SetName(name);
            Enable();
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Webhook name can not be empty.", nameof(name));
            }
            if (name.Length < 3)
            {
                throw new ArgumentException("Webhook name is too short.", nameof(name));
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("Webhook name is too long.", nameof(name));
            }
            if (!NameRegex.IsMatch(name))
            {
                throw new ArgumentException("Webhook name doesn't match the required criteria.", nameof(name));
            }

            Name = name.Trim().ToLowerInvariant();
            Endpoint = Name.Replace(" ", "-");
        }

        public void CreateToken()
        {
            Token = Guid.NewGuid().ToString();
        }

        public void ClearToken()
        {
            Token = null;
        }

        public void AddAction(WebhookAction action)
        {
            _actions.Add(action);
        }

        public void RemoveAction(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Webhook action name can not be empty.", nameof(name));
            }

            var action = _actions.SingleOrDefault(x => x.Name.Equals(name.Trim().ToLowerInvariant()));
            if(action == null)
            {
                return;
            }
            _actions.Remove(action);
        }

        public void AddTrigger(WebhookTrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public void RemoveTrigger(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Webhook trigger name can not be empty.", nameof(name));
            }

            var trigger = _triggers.SingleOrDefault(x => x.Name.Equals(name.Trim().ToLowerInvariant()));
            if(trigger == null)
            {
                return;
            }
            _triggers.Remove(trigger);
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