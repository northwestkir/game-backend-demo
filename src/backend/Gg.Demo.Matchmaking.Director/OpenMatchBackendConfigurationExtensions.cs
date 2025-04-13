using Google.Protobuf.WellKnownTypes;
using OpenMatch;

namespace Gg.Demo.Matchmaking.Director;

public static class OpenMatchBackendConfigurationExtensions
{
    public static IServiceCollection ConfigureDirector(this IServiceCollection services)
    {
        services.AddHostedService<FetchMatchesBackgroundService>();
        services.AddSingleton<OpenMatchBackendClient>();
        services.AddSingleton<IProfileRepository, ProfileRepository>();
        services.RegisterGrpcClient<BackendService.BackendServiceClient>("openmatch-backend", "OpenMatch:Backend");
        
        services.AddOptions<FunctionConfig>().BindConfiguration("OpenMatch:FunctionConfig").ValidateOnStart();
        return services;
    }

}
