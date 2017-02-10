using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medium.Configuration;
using Medium.Domain;
using Medium.Integrations.AspNetCore.Configuration;
using Medium.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Medium.Integrations.AspNetCore
{
    public static class Extensions
    {
        public static IMediumConfigurator AddMedium(this IServiceCollection services)
        {
            var container = new ServiceContainer(services);
            var resolver = new WebhookTriggerValidatorResolver();
            container.RegisterSingleton<IHttpClient, CustomHttpClient>();
            container.RegisterSingleton<IWebhookTriggerValidatorResolver>(resolver);
            container.RegisterTransient<IWebhookService, WebhookService>();
            var configurator = new MediumConfigurator(container, resolver);

            return configurator;
        }

        public static IApplicationBuilder UseMedium(this IApplicationBuilder app)
        {
            return app;
        }

        public static IApplicationBuilder UseMedium(this IApplicationBuilder app, IConfiguration configuration)
        {
            var configurator = app.ApplicationServices.GetService<IMediumConfigurator>();
            var settings = new Integrations.AspNetCore.Configuration.Medium();
            configuration.Bind(settings);
            var webhooks = settings.Webhooks.Select(x => MapWebhook(x));
            foreach(var webhook in webhooks)
            {
                configurator.AddWebhook(webhook);
            }

            var mediumConfiguration = configurator.Configure();
            var repository = mediumConfiguration.Repository;
            var tasks = new List<Task>();
            foreach(var webhook in mediumConfiguration.Webhooks)
            {
                tasks.Add(repository.AddAsync(webhook));
            }
            Task.WaitAll(tasks.ToArray());

            return app;
        }

        private static Webhook MapWebhook(WebhookModel model)
        {
            var webhook = new Webhook(model.Name);
            if(!string.IsNullOrWhiteSpace(model.Token))
            {
                webhook.SetToken(model.Token);
            }
            foreach(var action in model.Actions)
            {
                webhook.AddAction(MapWebhookAction(action));
            }
            foreach(var trigger in model.Triggers)
            {   
                webhook.AddTrigger(MapWebhookTrigger(trigger));
            }

            return webhook;
        }

        private static WebhookAction MapWebhookAction(WebhookActionModel model)
        {
            var action = new WebhookAction(model.Name, model.Url, model.Request);

            return action;
        }

        private static WebhookTrigger MapWebhookTrigger(WebhookTriggerModel model)
        {
            var trigger = WebhookTrigger.Create(model.Name, model.Type);

            return trigger;
        }
    }
}