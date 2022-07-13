using AlgoBacktesterBackend.Api.Services;
using AlgoBacktesterBackend.Domain.Repository;

namespace AlgoBacktesterBackend.Api.Config;
public static class ServiceBootstrapper {
    public static IServiceCollection AddServices(this IServiceCollection services, ApplicationSettings settings) {
        //Inject ApplicationSettings
        services.AddSingleton<ApplicationSettings>(settings);

        //Add transient services below + services which needs mandatory parameters in constructor
        //services.AddTransient<IDummyAdapter>(s => new DummyAdapter(string.Empty));

        services.AddTransient<IInformationService, InformationService>();
        services.AddTransient<IBacktestingService, BacktestingService>();
        services.AddTransient<IAssetPairService, AssetPairService>();
        
        //Add scoped service below
        services.AddScoped<IAssetPairRepository, AssetPairRepository>();
        
        return services;
    }
}