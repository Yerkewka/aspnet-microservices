using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddApplicationServices();
services.AddInfrastructureServices(builder.Configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:Host"]);
        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });

    config.AddConsumer<BasketCheckoutConsumer>();
});
services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddScoped<BasketCheckoutConsumer>();

var app = builder.Build();

app.Services.MigrateDatabase<DataContext>((context, services) =>
{
    var logger = services.GetRequiredService<ILogger<DataContextSeed>>();

    DataContextSeed.SeedAsync(context, logger).Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
