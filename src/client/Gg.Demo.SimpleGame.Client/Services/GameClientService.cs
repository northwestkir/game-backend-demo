using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gg.Demo.SimpleGame.Client.Services;

public class GameClientService : BackgroundService
{
    private readonly ILogger<GameClientService> _logger;
    private readonly IConfiguration _configuration;
    private readonly UdpClient _udpClient;
    private readonly string _serverHost;
    private readonly int _serverPort;

    public GameClientService(ILogger<GameClientService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _serverHost = _configuration.GetValue<string>("GameServer:Host", "localhost");
        _serverPort = _configuration.GetValue<int>("GameServer:Port", 7654);
        _udpClient = new UdpClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Game Client started. Server: {Host}:{Port}", _serverHost, _serverPort);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.Write("Enter message (or 'exit' to quit): ");
                var message = Console.ReadLine();

                if (string.Equals(message, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (string.IsNullOrEmpty(message))
                {
                    continue;
                }

                await SendAndReceiveMessage(message, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when stopping
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Game Client");
        }
    }

    private async Task SendAndReceiveMessage(string message, CancellationToken cancellationToken)
    {
        try
        {
            var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            await _udpClient.SendAsync(messageBytes, messageBytes.Length, _serverHost, _serverPort);

            var response = await _udpClient.ReceiveAsync(cancellationToken);
            var responseMessage = System.Text.Encoding.UTF8.GetString(response.Buffer);
            
            _logger.LogInformation("Received response: {Response}", responseMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending/receiving message");
        }
    }

    public override void Dispose()
    {
        _udpClient.Dispose();
        base.Dispose();
    }
} 