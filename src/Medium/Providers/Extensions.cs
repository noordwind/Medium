using Medium.Configuration;
using Medium.Domain;

namespace Medium.Providers
{
    public static class Extensions
    {
        public static IMediumConfigurator AddProvider<TRequest>(this IMediumConfigurator configurator) where TRequest : IRequest
        {
            configurator.AddProvider(new Provider<TRequest>());

            return configurator;
        }
    }
}