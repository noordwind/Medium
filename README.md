# Medium

## Your medium amongst webhooks.

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/noordwind/Medium.svg?branch=master)](https://travis-ci.org/noordwind/Medium)
|develop            |[![develop branch build status](https://api.travis-ci.org/noordwind/Medium.svg?branch=develop)](https://travis-ci.org/noordwind/Medium/branches)


**What is Medium?**
----------------

The library built in order to help consume the webhooks that can be invoked from different services like build servers, package managers, source control systems, custom APIs etc. 
and based on the received input validate such requests and execute any type of actions that you would like.

For example, you might be using some service A that is capable of sending webhooks to the given URL. On the other hand, there could be another service B, that you would like to 
send a custom request to, based on the input from service A. This is where the **Medium** comes in handy - it can act as as a sort of middleware, that will process the 
request from service A, validate it based on the set of defined rules, transform (if needed) into a separate object and eventually send a request to the service B (or more services).

With **Medium** you can build any sort of pipeline that you wish (e.g. deployment strategy), based on the webhooks.


**Extensions**
----------------

|Name                      |Description   
|--------------------------|-----------------------------------------------------                                    
|ASP.NET Core integration  |Running Medium within [ASP.NET Core](https://www.asp.net/core) application.
|Lockbox integration       |Loading encrypted settings from [Lockbox](https://getlockbox.com).


**Quick start**
----------------

**ASP.NET Core**
----------------

Please note that *Medium* does not require to use *ASP.NET Core*, as it's a simple libary that can be included within any type of a project.

Install packages via NuGet:
```
dotnet add package Medium
dotnet add package Medium.Integrations.AspNetCore
```

Create a *medium.json* file in the main directory, where you can define the configuration:
```json
{
  "webhooks": [
    {
      "name": "My webhook",
      "endpoint": "my-endpoint",
      "token": "secret",
      "defaultHeaders": {
        "Content-Type": "application/json",
        "Acccept": "application/json"
      },
      "defaultRequest": { "greeting": "Hello from Medium!" },
      "actions": [
        {
          "name": "Send my request",
          "url": "http://localhost:5000/api/test"
        }
      ],
      "triggers": [{
        "name": "My trigger",
        "actions": ["Send my request"],
        "rules": {
          "default": {
            "message": {
              "value": "hello",
              "comparison": "equals"
            }, 
            "user.id": {
              "value": "1",
              "comparison": "equals"
            }, 
            "user.name": {
              "value": "piotr",
              "comparison": "equals"
            }
          }
        }
      }]
    }
  ]
}
```

Edit your *Startup.cs* file:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMedium()
            .AddInMemoryRepository();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    app.UseMedium()
       .UseMvc();
}
```

Create a new controller e.g. *WebhooksController.cs*:

```cs
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
```

And another one for the trigger action testing purposes:

```cs
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
```

Run the application and execute a following POST request to the URL: *http://localhost:5000/api/webhooks/my-endpoint?token=secret&trigger=my-trigger*
```
HTTP Headers:
Content-Type: application/json
```
```json
{
	"message": "hello",
	"user": {
		"id": 1, 
		"name": "piotr"
	}
}
```

You'll see */api/test* endpoint being invoked. And that's it - feel free to include more triggers, actions, different URLs, requests and so on.

In order to find out how to extend the configuration and define much more sophisticated scenarios of use [read the wiki](https://github.com/noordwind/Medium/wiki).


