using System;
using Medium.Domain;
using Medium.Integrations.AspNetCore;
using Medium.Providers;
using Medium.Providers.MyGet;
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
                    .AddProvider<MyRequest>()
                    .AddMyGetProvider()
                    .AddInMemoryRepository();

            services.AddMvc(options => options.InputFormatters.AddMyGetFormatter())
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

    public class MyRequest : IRequest
    {
        public Guid Identifier { get; set; }
        public string Username { get; set; }
        public DateTime When { get; set; }
    } 
}