namespace Medium.Configuration
{
    public interface IServiceContainer
    {
        void Register<TService>(TService service) where TService : class;
        void Register<TService,TImplementation>() where TService : class where TImplementation : class, TService;
        void RegisterSingleton<TService>(TService service) where TService : class;
        void RegisterSingleton<TService,TImplementation>() where TService : class where TImplementation : class, TService;
        void RegisterTransient<TService>(TService service) where TService : class;
        void RegisterTransient<TService,TImplementation>() where TService : class where TImplementation : class, TService;       
    }
}