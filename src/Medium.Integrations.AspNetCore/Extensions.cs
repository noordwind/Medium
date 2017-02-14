using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medium.Configuration;
using Medium.Domain;
using Medium.Integrations.AspNetCore.Configuration;
using Medium.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
            => app.UseMedium("medium.json");

        public static IApplicationBuilder UseMedium(this IApplicationBuilder app, string configurationFilePath)
            => app.UseMedium(x => x.SettingsLoader = new MediumFileSettingsLoader(configurationFilePath));

        public static IApplicationBuilder UseMedium(this IApplicationBuilder app, Action<MediumOptions> options)
        {
            var configurator = app.ApplicationServices.GetService<IMediumConfigurator>();
            var mediumOptions = new MediumOptions();
            options(mediumOptions);
            var configuration = mediumOptions.SettingsLoader.Load();
            var settings = JsonConvert.DeserializeObject<MediumSettings>(configuration);
            var webhooks = settings.Webhooks.Select(x => MapWebhook(x));
            foreach(var webhook in webhooks)
            {
                configurator.AddWebhook(webhook);
            }

            var mediumConfiguration = configurator.Configure();
            if(!mediumConfiguration.Webhooks.Any())
            {
                return app;
            }

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
            var webhook = new Webhook(model.Name, model.Endpoint);
            if(model.Inactive)
            {
                webhook.Deactivate();
            }
            if(!string.IsNullOrWhiteSpace(model.Token))
            {
                webhook.SetToken(model.Token);
            }
            webhook.SetDefaultRequest(model.DefaultRequest);
            foreach (var header in model.DefaultHeaders ?? new Dictionary<string, object>())
            {
                webhook.DefaultHeaders[header.Key] = header.Value;
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
            action.SetCodename(model.Codename);
            if(model.Inactive)
            {
                action.Deactivate();
            }
            foreach (var header in model.Headers ?? new Dictionary<string, object>())
            {
                action.Headers[header.Key] = header.Value;
            }

            return action;
        }

        private static WebhookTrigger MapWebhookTrigger(WebhookTriggerModel model)
        {
            var trigger = WebhookTrigger.Create(model.Name, model.Type);
            trigger.SetRules(model.Rules);
            if(model.Inactive)
            {
                trigger.Deactivate();
            }
            foreach (var action in model.Actions ?? Enumerable.Empty<string>())
            {
                trigger.AddAction(action);
            }
            foreach (var requester in model.Requesters ?? Enumerable.Empty<string>())
            {
                trigger.AddRequester(requester);
            }

            return trigger;
        }
    }
}