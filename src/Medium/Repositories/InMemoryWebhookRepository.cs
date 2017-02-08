using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medium.Domain;

namespace Medium.Repositories
{
    public class InMemoryWebhookRepository : IWebhookRepository
    {
        private static readonly ISet<Webhook> _webhooks = new HashSet<Webhook>();

        public async Task<Webhook> GetAsync(string endpoint)
            => await Task.FromResult(_webhooks.SingleOrDefault(x => x.Endpoint == endpoint.ToLowerInvariant()));

        public async Task<Webhook> GetAsync(Guid id)
            => await Task.FromResult(_webhooks.SingleOrDefault(x => x.Id == id));

        public async Task<IEnumerable<Webhook>> GetAllAsync() 
             => await Task.FromResult(_webhooks);

        public async Task AddAsync(Webhook webhook)
        {
            await Task.FromResult(_webhooks.Add(webhook));
        }

        public async Task DeleteAsync(Guid id)
        {
            var webhook = _webhooks.SingleOrDefault(x => x.Id == id);
            if(webhook == null)
            {
                return;
            }

            await Task.FromResult(_webhooks.Remove(webhook));
        }

        public async Task UpdateAsync(Webhook webhook)
            => await Task.CompletedTask;
    }
}