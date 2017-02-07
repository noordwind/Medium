namespace Medium.Domain
{
    public class WebhookAction
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }
        public object RequestBody { get; set; }

        protected WebhookAction()
        {
        }

        public WebhookAction(string name, string url, object requestBody = null)
        {
            Name = name;
            Url = url;
            RequestBody = requestBody;
            Enabled = true;
        }
    }
}