using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gg.Demo.SimpleGameServer.Host.Services;
public class UdpGameServerServiceOptions
{
    public int Port { get; set; } = 7654;
}
public class UdpGameServerService : BackgroundService
{
    private readonly ILogger<UdpGameServerService> _logger;
    private readonly UdpClient _udpClient;
    private readonly int _port;

    public UdpGameServerService(ILogger<UdpGameServerService> logger, IOptions<UdpGameServerServiceOptions> options)
    {
        _logger = logger;
        _port = options.Value.Port;
        _udpClient = new UdpClient(_port);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("UDP Game Server started listening on port {Port}", _port);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _udpClient.ReceiveAsync(stoppingToken);
                var message = System.Text.Encoding.UTF8.GetString(result.Buffer);
                
                _logger.LogInformation("Received message from {RemoteEndPoint}: {Message}", 
                    result.RemoteEndPoint, message);

                var response = $"Me answers: {message}";
                var responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
                
                await _udpClient.SendAsync(responseBytes, responseBytes.Length, result.RemoteEndPoint);
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when stopping
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UDP Game Server");
        }
    }

    public override void Dispose()
    {
        _udpClient.Dispose();
        base.Dispose();
    }
} 