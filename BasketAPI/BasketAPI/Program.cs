using Basket.API.Repositories;
using MassTransit;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Add services to the container

// Add Redis
var redisConnection = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
Console.WriteLine($"Connecting to Redis at {redisConnection}");

// Add Repository
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Add RabbitMQ with MassTransit
builder.Services.AddMassTransit(config =>
{
     // Register the consumer

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

       
    });
});

builder.Services.AddMassTransitHostedService();


// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Basket API",
        Version = "v1",
        Description = "Basket API for managing shopping baskets",
        Contact = new OpenApiContact
        {
            Name = "Support Team",
            Email = "support@example.com",
            Url = new Uri("https://example.com")
        }
    });
});

// Build the app
var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
    });
}

app.UseAuthorization();
app.MapDefaultControllerRoute(); // Add this line to enable default routing
app.MapControllers();
app.Run();

