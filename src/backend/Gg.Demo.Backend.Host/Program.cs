using Gg.Demo.Backend.Core.Matchmaking;
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


builder.Services.ConfigureOpenMatchFrontEndClient();

using var app = builder.Build();

app.Run();