using System.Collections.Generic;

namespace Medium.Domain
{
    public class WebhookTrigger
    {
        private ISet<string> _requesters = new HashSet<string>();  
        public string Name { get; set; }
        public string Type { get; set; }
        public object Rules { get; set; }
        public bool Enabled { get; set; }

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

        public WebhookTrigger(string name, object rules)
        {
            Name = name;
            Rules = rules;
            Enabled = true;
        }
    }
}