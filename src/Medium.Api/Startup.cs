using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Medium.Integrations.AspNetCore;
using Medium.Integrations.AspNetCore.Configuration;
using Medium.Integrations.Lockbox;
using Medium.Providers.MyGet;
using Medium.Repositories;
using Medium.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMedium()
                    .AddMyGetProvider()
                    .AddInMemoryRepository();

            services.AddMvc()
                    .AddJsonOptions(x => 
                    {
                        x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        x.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
                        x.SerializerSettings.Formatting = Formatting.Indented;
                        x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        x.SerializerSettings.Converters.Add(new StringEnumConverter
                        {
                            AllowIntegerValues = true,
                            CamelCaseText = true
                        });
                    });;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMedium();
            app.UseMvc();
        }
    }
}