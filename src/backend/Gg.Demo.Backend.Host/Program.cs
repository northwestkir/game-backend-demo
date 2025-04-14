using System.Net;
using Gg.Demo.Backend.Core.Matchmaking;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(static siloBuilder =>
{
    siloBuilder.UseKubernetesHosting(options => options.ValidateOnStart());
    
    siloBuilder.AddMemoryGrainStorage("matchmakingStore");
    siloBuilder.Configure<EndpointOptions>(options =>
{
    // Port to use for silo-to-silo
    options.SiloPort = 11_111;
    // Port to use for the gateway
    options.GatewayPort = 30_000;
    // IP Address to advertise in the cluster
    options.AdvertisedIPAddress = IPAddress.Parse("172.16.0.42");
    // The socket used for client-to-silo will bind to this endpoint
    options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 40_000);
    // The socket used by the gateway will bind to this endpoint
    options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 50_000);
    });
});


builder.Services.ConfigureOpenMatchFrontEndClient();

using var app = builder.Build();

app.Run();