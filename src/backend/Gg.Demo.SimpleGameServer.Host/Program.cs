using Gg.Demo.SimpleGameServer.Host.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register UDP Game Server Service
builder.Services
    .AddHostedService<UdpGameServerService>()
    .AddOptions<UdpGameServerServiceOptions>().BindConfiguration("GameServer").ValidateOnStart().Validate(options => options.Port > 0);

var app = builder.Build();

app.Run(); 