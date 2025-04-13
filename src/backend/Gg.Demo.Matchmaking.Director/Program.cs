using Gg.Demo.Allocator.Abstractions;
using Gg.Demo.Matchmaking.Director;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOpenMatchBackend();
builder.Services.AddLocalGameServerAllocator("LocalGameServer");

var app = builder.Build();

app.Run();
