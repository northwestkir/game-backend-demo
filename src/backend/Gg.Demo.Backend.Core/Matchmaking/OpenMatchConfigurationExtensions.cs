using Microsoft.Extensions.DependencyInjection;
using OpenMatch;
using Gg.Demo.Matchmaking;

namespace Gg.Demo.Backend.Core.Matchmaking;

public static class OpenMatchConfigurationExtensions
{
    public static void ConfigureOpenMatchFrontEndClient(this IServiceCollection services)
    {
        services.AddScoped<OpenMatchFrontendClient>();
        services.RegisterGrpcClient<FrontendService.FrontendServiceClient>("om-frontend", "OpenMatchFrontEnd");
    }
}
