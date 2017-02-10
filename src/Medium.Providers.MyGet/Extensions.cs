using Medium.Configuration;

namespace Medium.Providers.MyGet
{
    public static class Extensions
    {
        public static IMediumConfigurator AddMyGetProvider(this IMediumConfigurator configurator)
        {
            configurator.AddProvider(new MyGetProvider());

            return configurator;
        }
    }
}