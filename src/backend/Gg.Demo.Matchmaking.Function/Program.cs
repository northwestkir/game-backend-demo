using Gg.Demo.Matchmaking.Function;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.ConfigureOpenMatchFunction();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<GrpcMatchFunctionService>();

app.Run();
