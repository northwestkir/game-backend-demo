using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gg.Demo.Allocator.Abstractions;

public interface IGameServerAllocator
{
    Task<GameServer> AllocateGameServer(MatchInfo match, CancellationToken cancellationToken);
}

public class StaticGameServerAllocator(IOptions<GameServer> gameServerOptions, ILogger<StaticGameServerAllocator> logger) : IGameServerAllocator
{
    public async Task<GameServer> AllocateGameServer(MatchInfo match, CancellationToken cancellationToken)
    {
        logger.LogDebug("Allocating game server for match: {MatchId}", match.MatchId);
        await Task.Delay(5000, cancellationToken);
        return gameServerOptions.Value;
    }
}

public static class GameServerAllocatorExtensions
{
    public static IServiceCollection AddLocalGameServerAllocator(this IServiceCollection services, string sectionName)    
    {
        services.AddSingleton<IGameServerAllocator, StaticGameServerAllocator>();
        services.AddOptions<GameServer>().BindConfiguration(sectionName);
        return services;
    }
}