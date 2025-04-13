using System;
using Gg.Demo.Matchmaking;
using OpenMatch;
namespace Gg.Demo.Matchmaking.Function;

public static class OpenMatchConfigurationExtensions
{
    public static IServiceCollection ConfigureOpenMatchFunction(this IServiceCollection services)
    {
        services.RegisterGrpcClient<QueryService.QueryServiceClient>("openmatch-query", "OpenMatch:Query");
        return services;
    }
}
