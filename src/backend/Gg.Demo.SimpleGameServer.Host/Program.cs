using Agones;
using Gg.Demo.SimpleGameServer.Host.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<AgonesService>();
        services.AddSingleton<IAgonesSDK>(sp => new AgonesSDK());

        services.AddHostedService<UdpGameServerService>();
        services.AddOptions<UdpGameServerServiceOptions>().BindConfiguration("GameServer").ValidateOnStart().Validate(options => options.Port > 0);
    });

var host = builder.Build();
await host.RunAsync(); 