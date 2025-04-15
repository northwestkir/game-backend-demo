using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Gg.Demo.Allocator.Agones;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAgonesGameServerAllocator(this IServiceCollection services, string sectionName = "Agones")
    {
        services.AddOptions<AgonesAllocatorOptions>().BindConfiguration(sectionName);
        return services;
    }
} 