using Gg.Demo.FrontEnd.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Orleans.Configuration;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add authentication

builder.Services.AddMagicOnion();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddOrleansClient(clientBuilder =>
{
    clientBuilder.Configure<ClusterOptions>(options =>
    {
        options.ServiceId = builder.Configuration["Orleans:ServiceId"];
        options.ClusterId = builder.Configuration["Orleans:ClusterId"];
    });
    clientBuilder.UseRedisClustering(options =>
    {
        var it = builder.Configuration.GetValue<string>("Orleans:Clustering:Redis:ConnectionString")
        ?? throw new InvalidOperationException("OrleansRedis connection string not found");
        options.ConfigurationOptions = ConfigurationOptions.Parse(it);
    });
}
);

builder.Services.AddOptions<ClusterOptions>().PostConfigure<ILogger<ClusterOptions>>((options,Logger) =>
{
    Logger.LogInformation("Cluster options: {ServiceId} {ClusterId}", options.ServiceId, options.ClusterId);
});

var app = builder.Build();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapMagicOnionService<MagicOnionFrontEndService>();


app.Run();