# Medium

####**Your medium amongst webhooks.**

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/noordwind/Medium.svg?branch=master)](https://travis-ci.org/noordwind/Medium)
|develop            |[![develop branch build status](https://api.travis-ci.org/noordwind/Medium.svg?branch=develop)](https://travis-ci.org/noordwind/Medium/branches)


**What is Medium?**
----------------

It's a library built in order to help you consume the webhooks that can be invoked from different services like build servers, package managers, source control systems, custom APIs etc. 
and based on the received input validate such requests and execute any type of actions that you would like.

For example, you might be using a service A that is capable of sending webhooks to the given URL. On the other hand, there could be a service B, that you would like to 
send a custom request to, based on the input from service A. This is where the **Medium** comes in handy - it can act as as a sort of middleware, that will process the 
request from service A, validate it based on the set of defined rules, transform (if needed) into a separate object and eventually send a request to the service B (or more services).

With **Medium** you can build any sort of pipeline that you wish (e.g. deployment strategy), based on the webhooks.


**Extensions**
----------------

|Name                      |Description   
|--------------------------|-----------------------------------------------------                                    
|ASP.NET Core integration  |Running Medium within [ASP.NET Core](https://www.asp.net/core) application.
|Lockbox integration       |Loading encrypted settings from [Lockbox](https://github.com/Lockbox-stack/Lockbox).
|MyGet provider            |Requests and validation rules for [MyGet](https://www.myget.org/) webhooks.


**Quick start**
----------------

**ASP.NET Core**
----------------

Install packages via NuGet:
```
Install-Package Medium -Pre
Install-Package Medium.Integrations.AspNetCore -Pre
```

Create a *medium.json* file in the main directory, where you can define your configuration:
```json
TODO
```

Edit your *Startup.cs* file:
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMedium()
            .AddMyGetProvider()
            .AddInMemoryRepository();

    services.AddMvc(options => options.InputFormatters.AddMyGetFormatter());
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    app.UseMedium();
    app.UseMvc();
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

Run the application and send a POST request to the following URL: *http://localhost:5000/api/webhooks/my-endpoint?token=secret&trigger=test-trigger*

That's it!


