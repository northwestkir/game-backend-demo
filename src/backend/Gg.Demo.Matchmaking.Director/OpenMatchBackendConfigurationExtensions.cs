using OpenMatch;

namespace Gg.Demo.Matchmaking.Director;

public static class OpenMatchBackendConfigurationExtensions
{
    public static IServiceCollection ConfigureOpenMatchBackend(this IServiceCollection services)
    {
        services.AddHostedService<FetchMatchesBackgroundService>();
        services.AddSingleton<OpenMatchBackendClient>();
        services.RegisterGrpcClient<BackendService.BackendServiceClient>("openmatch-backend", "OpenMatch:Backend");
        services.AddOptions<MatchProfile>().BindConfiguration("OpenMatch:MatchProfile").ValidateOnStart();
        services.AddOptions<FunctionConfig>().BindConfiguration("OpenMatch:FunctionConfig").ValidateOnStart();
        return services;
    }
}
