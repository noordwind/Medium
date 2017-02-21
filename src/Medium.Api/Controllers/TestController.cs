using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Medium.Api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Post([FromBody]object request)
        {
            _logger.LogInformation("Test action was triggered by webhook.");
        }
    }
}