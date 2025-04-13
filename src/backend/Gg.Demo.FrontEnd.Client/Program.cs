using Grpc.Net.Client;
using MagicOnion.Client;
using Gg.Demo.FrontEnd.Common;
using Grpc.Core;

namespace Gg.Demo.FrontEnd.Client;

class Program
{
    static async Task Main(string[] args)
    {
        // Example usage:
        var jwtKey = "your-secret-key-here-must-be-at-least-16-characters";
        var userId = Guid.Empty;
        var username = "testuser";

        var token = JwtTokenGenerator.GenerateJwtToken(jwtKey, userId, username);
        Console.WriteLine($"Generated JWT Token: {token}");

        // Connect to the server using gRPC channel.
        var channel = GrpcChannel.ForAddress("http://localhost:5222");

        // Create metadata with the JWT token
        var metadata = new Metadata
        {
            { "Authorization", $"Bearer {token}" }
        };

        // Create a proxy to call the server transparently with the JWT token
        var client = MagicOnionClient.Create<IFrontEndService>(channel).WithHeaders(metadata);

        try
        {
            // Call the service.
            var result = await client.GetMatchmakingState();
            Console.WriteLine($"Matchmaking result: {result.Status}");
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"RPC Error: {ex.Status.StatusCode} - {ex.Status.Detail}");
        }

        // Wait for user input before closing the console window.
        Console.ReadLine();
    }
}