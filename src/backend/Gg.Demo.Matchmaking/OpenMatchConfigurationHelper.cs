using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gg.Demo.Matchmaking;

public static class OpenMatchConfigurationHelper
{

    public static void RegisterGrpcClient<TClient>(this IServiceCollection services, string clientName, string configPath) where TClient : class
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
