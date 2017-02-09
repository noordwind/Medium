using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medium.Integrations.MyGet;
using Medium.Repositories;
using Medium.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Medium.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            var validatorResolver = new WebhookTriggerValidatorResolver();
            validatorResolver.Register<MyGetPackageAddedRequest>((request,rules)  => 
                new MyGetPackageAddedValidator().Validate((MyGetPackageAddedRequest)request, (MyGetPackageAddedRules)rules));

            services.AddTransient<IWebhookService, WebhookService>();
            services.AddSingleton<IHttpClient, CustomHttpClient>();
            services.AddSingleton<IWebhookTriggerValidatorResolver>(validatorResolver);
            var repository = new InMemoryWebhookRepository();
            var provider = new SampleWebhooksProvider();
            var tasks = new List<Task>();
            foreach(var webhook in provider.GetAll())
            {
                tasks.Add(repository.AddAsync(webhook));
            }
            Task.WaitAll(tasks.ToArray());
            services.AddTransient<IWebhookRepository>(x => repository);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMvc();
        }
    }
}