
using Gg.Demo.FrontEnd.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMagicOnion();
builder.Services.AddHttpContextAccessor();

builder.Services.AddOrleansClient(orleans => orleans
    .UseLocalhostClustering(serviceId: "backend", clusterId: "dev"));

var app = builder.Build();

app.MapMagicOnionService<MagicOnionFrontEndService>(); 

app.Run();