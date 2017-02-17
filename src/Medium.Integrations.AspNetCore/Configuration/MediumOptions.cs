using Medium.Configuration;

namespace Medium.Integrations.AspNetCore.Configuration
{
    public class MediumOptions
    {
        public IMediumSettingsLoader SettingsLoader { get; set; }
    }
}