using Gg.Demo.Backend.Core.Matchmaking;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(static siloBuilder =>
{
    siloBuilder.UseLocalhostClustering(serviceId: "backend", clusterId: "dev");
    siloBuilder.AddMemoryGrainStorage("matchmakingStore");    
});

builder.Services.ConfigureOpenMatchFrontEndClient();

using var app = builder.Build();

app.Run();