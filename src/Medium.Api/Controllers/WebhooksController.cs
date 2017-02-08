using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medium.Domain;
using Medium.Integrations.MyGet;
using Medium.Services;
using Microsoft.AspNetCore.Mvc;

namespace Medium.Api.Controllers
{
    [Route("api/[controller]")]
    public class WebhooksController : Controller
    {
        private readonly IWebhookService _webhookService;

        public WebhooksController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Webhook>> Get()
        {
            return await _webhookService.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]object request)
        {
        }

        [HttpPost("{endpoint}")]
        public async Task Post(string endpoint, string trigger, [FromBody]object request, string token)
        {
            await _webhookService.ExecuteAsync(endpoint, trigger, request, token);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}