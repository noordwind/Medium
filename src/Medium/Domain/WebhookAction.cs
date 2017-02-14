using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Medium.Domain
{
    public class WebhookAction
    {
        private static readonly Regex NameRegex = new Regex("([a-zA-Z1-9 _\\-])\\w+", RegexOptions.Compiled);
        public string Name { get; protected set; }
        public string Codename { get; protected set; }
        public string Url { get; protected set; }
        public object Request { get; protected set; }
        public IDictionary<string, object> Headers  { get; protected set; } = new Dictionary<string, object>();
        public bool Inactive { get; protected set; }

        protected WebhookAction()
        {
        }

        public WebhookAction(string name, string url, object request = null)
        {
            SetName(name);
            SetCodename(name);
            SetUrl(url);
            SetRequest(request);
            Activate();
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Webhook action name can not be empty.", nameof(name));
            }
            if (name.Length < 3)
            {
                throw new ArgumentException("Webhook action name is too short.", nameof(name));
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("Webhook action name is too long.", nameof(name));
            }
            if (!NameRegex.IsMatch(name))
            {
                throw new ArgumentException("Webhook action name doesn't match the required criteria.", nameof(name));
            }

            Name = name.Trim().ToLowerInvariant();
        }

        public void SetCodename(string codename)
        {
            if(string.IsNullOrWhiteSpace(codename))
            {
                return;
            }
            Codename = codename.Trim().Replace(" ", "-").ToLowerInvariant();
        }

        public void SetUrl(string url)
        {
            new Uri(url);
            Url = url;
        }

        public void ClearRequest()
        {
            SetRequest(null);
        }

        public void SetRequest(object request)
        {
            Request = request;
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