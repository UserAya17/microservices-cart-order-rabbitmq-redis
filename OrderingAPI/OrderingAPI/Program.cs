using Microsoft.EntityFrameworkCore;
using OrderingAPI.Data;
using MassTransit;
using OrderingAPI.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Ajoutez tous les services ici, avant builder.Build()
builder.Services.AddControllersWithViews(); // Activer les contr�leurs et vues Razor

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint("basket-checkout-queue", c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});
builder.Services.AddMassTransitHostedService();

builder.Services.AddDbContext<OrderingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderingConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cr�ez l'application apr�s avoir enregistr� tous les services
var app = builder.Build();
app.UseStaticFiles();

// Configurez le routage des contr�leurs et des vues Razor
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Seed de la base de donn�es
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<OrderingContext>();
    OrderingAPI.Controllers.OrderController.Seed(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
