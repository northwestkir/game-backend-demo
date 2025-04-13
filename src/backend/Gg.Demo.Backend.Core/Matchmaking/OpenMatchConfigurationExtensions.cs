using Microsoft.Extensions.DependencyInjection;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Logging;
using OpenMatch;

namespace Gg.Demo.Backend.Core.Matchmaking;

public static class OpenMatchConfigurationExtensions
{
    public static void ConfigureOpenMatchFrontEndClient(this IServiceCollection services)
    {
        services.AddScoped<OpenMatchFrontendClient>();
        services.RegisterGrpcClient<FrontendService.FrontendServiceClient>("om-frontend", "OpenMatchFrontEnd");
    }


    static void RegisterGrpcClient<TClient>(this IServiceCollection services, string clientName, string configPath) where TClient : class
    {
        services
            .AddOptions<GrpcClientFactoryOptions>(clientName)
            .BindConfiguration(configPath)
            .Validate(o => o.Address != null, $"{configPath}:Address is required")
            .ValidateOnStart()
            .PostConfigure<ILogger<GrpcClientFactoryOptions>>((o, logger) =>
                logger.LogInformation("{ConfigPath}:Address: {Address}", configPath, o.Address))
            .Services
            .AddGrpcClient<TClient>(clientName);
    }
}
