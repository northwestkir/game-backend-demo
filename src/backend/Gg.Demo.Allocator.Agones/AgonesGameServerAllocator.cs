using System.Net.Http.Headers;
using System.Text.Json;
using Agones;
using Agones.Dev.Sdk;
using Gg.Demo.Allocator.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gg.Demo.Allocator.Agones;

public class AgonesGameServerAllocator : IGameServerAllocator
{
    private readonly IAgonesSDK _agonesSDK;
    private readonly AgonesAllocatorOptions _options;
    private readonly ILogger<AgonesGameServerAllocator> _logger;

    public AgonesGameServerAllocator(
        IAgonesSDK agonesSDK,
        IOptions<AgonesAllocatorOptions> options,
        ILogger<AgonesGameServerAllocator> logger)
    {
        _agonesSDK = agonesSDK;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<GameServerInfo> AllocateGameServer(MatchInfo match, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Allocating game server for match {MatchId}", match.MatchId);       
        throw new NotImplementedException();
    }

    private class AllocationResponse
    {
        public Status? Status { get; set; }
    }

    private class Status
    {
        public string? State { get; set; }
        public string? Address { get; set; }
        public List<GameServerPort> Ports { get; set; } = new();
    }

    private class GameServerPort
    {
        public string Name { get; set; } = string.Empty;
        public int Port { get; set; }
    }
} 