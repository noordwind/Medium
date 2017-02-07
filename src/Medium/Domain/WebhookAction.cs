using System;
using System.Text.RegularExpressions;

namespace Medium.Domain
{
    public class WebhookAction
    {
        private static readonly Regex NameRegex = new Regex("([a-zA-Z1-9 _\\-])\\w+", RegexOptions.Compiled);
        public string Name { get; protected set; }
        public string Url { get; protected set; }
        public object RequestBody { get; protected set; }
        public bool Enabled { get; protected set; }

        protected WebhookAction()
        {
        }

        public WebhookAction(string name, string url, object requestBody = null)
        {
            SetName(name);
            SetUrl(url);
            SetRequestBody(requestBody);
            Enable();
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

        public void SetUrl(string url)
        {
            new Uri(url);
            Url = url;
        }

        public void SetRequestBody(object requestBody)
        {
            RequestBody = requestBody;
        }

        public void ClearRequestBody()
        {
            RequestBody = null;
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