
using Grpc.Net.Client;
using MagicOnion.Client;
using Gg.Demo.FrontEnd.Common;

// Connect to the server using gRPC channel.
var channel = GrpcChannel.ForAddress("http://localhost:5222");

// Create a proxy to call the server transparently.
var client = MagicOnionClient.Create<IFrontEndService>(channel);

// Call the server-side method using the proxy.
var result = await client.GetMatchmakingState();
Console.WriteLine($"Result: {result}");