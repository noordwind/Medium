using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medium.Configuration;
using Medium.Configuration.Models;
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
            container.RegisterSingleton<IHttpClient, CustomHttpClient>();
            container.RegisterTransient<IWebhookService, WebhookService>();
            var configurator = new MediumConfigurator(container);

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
            var webhooks = settings.Webhooks.Select(x => WebhookModel.MapToWebhook(x));
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
    }
}