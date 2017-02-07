using System;
using System.Collections.Generic;

namespace Medium.Domain
{
    public class Webhook
    {
        private ISet<WebhookAction> _actions = new HashSet<WebhookAction>();  
        private ISet<WebhookTrigger> _triggers = new HashSet<WebhookTrigger>();  

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Endpoint { get; set; }
        public string Token { get; set; }
        public bool Enabled { get; set; }

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

        public Webhook(string name, string token = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Enabled = true;
        }
    }
}