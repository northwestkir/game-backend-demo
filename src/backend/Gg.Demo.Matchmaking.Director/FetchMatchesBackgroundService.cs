using Gg.Demo.Allocator.Abstractions;

namespace Gg.Demo.Matchmaking.Director;

public class FetchMatchesBackgroundService(OpenMatchBackendClient client, IGameServerAllocator gameServerAllocator, ILogger<FetchMatchesBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await foreach (var match in client.FetchMatches("default", stoppingToken))
                {
                    await ProcessMatch(match, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while fetching matches");
            }
            await Task.Delay(10000, stoppingToken);
        }
    }

    private async Task ProcessMatch(MatchInfo match, CancellationToken stoppingToken)
    {
        var step = "";
        try
        {
            step = "Fetching match";
            logger.LogDebug("Match: {@Match} for {MatchId}", match, match.MatchId);
            var gameServer = await gameServerAllocator.AllocateGameServer(match, stoppingToken);

            step = "Assigning tickets";
            logger.LogDebug("GameServer: {@GameServer} for {MatchId}", gameServer, match.MatchId);
            await client.AssignTickets(match, gameServer, stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling match: {MatchId} at step {Step}", match.MatchId, step);
        }
    }
}
