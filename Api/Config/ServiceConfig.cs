using AlgoBacktesterBackend.Api.Services;
using AlgoBacktesterBackend.Api.Adapters;

namespace AlgoBacktesterBackend.Api.Config;
public static class ServiceBootstrapper {
    public static IServiceCollection AddServices(this IServiceCollection services, ApplicationSettings settings) {
        //Add transient services below + services which needs mandatory parameters in constructor
        //services.AddTransient<IDummyAdapter>(s => new DummyAdapter(string.Empty));
        
        //Add scoped service below
        services.AddScoped<IInformationService, InformationService>();
        services.AddScoped<IBacktestingService, BacktestingService>();
        return services;
    }
}