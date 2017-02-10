using System.Collections.Generic;
using System.Threading.Tasks;
using Medium.Domain;
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

        [HttpGet]
        public async Task<IEnumerable<Webhook>> Get()
        {
            return await _webhookService.GetAllAsync();
        }

        [HttpPost("{endpoint}")]
        public async Task Post(string endpoint, string trigger, [FromBody]object request, string token)
        {
            await _webhookService.ExecuteAsync(endpoint, trigger, request, token);
        }
    }
}