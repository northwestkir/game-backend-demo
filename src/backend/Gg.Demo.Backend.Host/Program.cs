using System.Net;
using Gg.Demo.Backend.Core.Matchmaking;
using Orleans.Configuration;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(static siloBuilder =>
{
    if (siloBuilder.Configuration.GetValue<bool>("Orleans:UseKubernetesHosting"))
        siloBuilder.UseKubernetesHosting(options => options.ValidateOnStart());

    siloBuilder.UseRedisClustering(options =>
    {
        var it = siloBuilder.Configuration.GetSection("Orleans:Clustering:Redis:ConnectionString").Get<string>()
            ?? throw new InvalidOperationException("OrleansRedis connection string not found");
        options.ConfigurationOptions = ConfigurationOptions.Parse(it);
    });

    siloBuilder.AddMemoryGrainStorage("matchmakingStore");
});

builder.Services.AddOptions<EndpointOptions>().PostConfigure<ILogger<EndpointOptions>>((options, Logger) =>
{
    options.GatewayListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.GatewayPort);
    Logger.LogInformation("Endpoint options: {Address} {Gateway}", options.AdvertisedIPAddress, options.GatewayListeningEndpoint);
});

builder.Services.AddOptions<ClusterOptions>().PostConfigure<ILogger<ClusterOptions>>((options,Logger) =>
{
    Logger.LogInformation("Cluster options: {ServiceId} {ClusterId}", options.ServiceId, options.ClusterId);
});

builder.Services.ConfigureOpenMatchFrontEndClient();

using var app = builder.Build();

app.Run();