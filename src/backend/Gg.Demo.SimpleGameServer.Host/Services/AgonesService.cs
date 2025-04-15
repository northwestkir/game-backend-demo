using Agones;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gg.Demo.SimpleGameServer.Host.Services;

public class AgonesService(IAgonesSDK agones, ILogger<AgonesService> logger) : IHostedService
{
    private readonly CancellationTokenSource _cts = new();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Marking server as ready...");
            await agones.ReadyAsync();

            agones.WatchGameServer((gameServer) => { logger.LogInformation("Game server updated: {GameServer}", gameServer); });

            // Start health check loop
            _ = Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        await agones.HealthAsync();
                        await Task.Delay(TimeSpan.FromSeconds(2), _cts.Token);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error in health check loop");
                    }
                }
            }, _cts.Token);

            logger.LogInformation("Agones service started successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error starting Agones service");
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        try
        {
            logger.LogInformation("Shutting down Agones service...");
            await agones.ShutDownAsync();
            logger.LogInformation("Agones service shut down successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error shutting down Agones service");
        }
    }
} 