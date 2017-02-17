using Medium.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Medium.Providers.MyGet
{
    public static class Extensions
    {
        public static IMediumConfigurator AddMyGetProvider(this IMediumConfigurator configurator)
        {
            configurator.AddProvider(new MyGetProvider());

            return configurator;
        }

        public static FormatterCollection<IInputFormatter> AddMyGetFormatter(this FormatterCollection<IInputFormatter> formatters)
        {
            formatters.Add(new MyGetInputFormatter());

            return formatters;
        }
        
    }
}