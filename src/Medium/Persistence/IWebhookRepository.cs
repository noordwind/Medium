using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Medium.Domain;

namespace Medium.Persistence
{
    public interface IWebhookRepository
    {
         Task<Webhook> GetAsync(Guid id);
         Task<Webhook> GetAsync(string endpoint);
         Task<IEnumerable<Webhook>> GetAllAsync();
         Task AddAsync(Webhook webhook);
         Task UpdateAsync(Webhook webhook);
         Task DeleteAsync(Guid id);
    }
}