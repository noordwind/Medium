using Medium.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Medium.Integrations.AspNetCore
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly IServiceCollection _services;

        public ServiceContainer(IServiceCollection services)
        {
            _services = services;
        }

        public void Register<TService>(TService service) where TService : class
        {
            _services.AddScoped<TService>(x => service);
        }

        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _services.AddScoped<TService, TImplementation>();
        }

        public void RegisterSingleton<TService>(TService service) where TService : class
        {
            _services.AddSingleton<TService>(service);
        }

        public void RegisterSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _services.AddSingleton<TService, TImplementation>();
        }

        public void RegisterTransient<TService>(TService service) where TService : class
        {
            _services.AddTransient<TService>(x => service);
        }

        public void RegisterTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _services.AddTransient<TService, TImplementation>();
        }
    }
}