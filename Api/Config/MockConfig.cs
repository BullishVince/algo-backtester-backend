using Serilog;
using AlgoBacktesterBackend.Api.Config.Mocks;

namespace AlgoBacktesterBackend.Api.Config;
public static class MockConfig {
    public static IServiceCollection AddMocks(this IServiceCollection services, ApplicationSettings applicationSettings) {
    if (applicationSettings.UseMocks) {
        Log.Information("Initiating mocks");

        //Add mocks below
        services.AddSingleton(InformationServiceMock.Get());
        services.AddSingleton(BacktestingServiceMock.Get());
    }
    return services;
    }
}