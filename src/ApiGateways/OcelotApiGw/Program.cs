using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

configuration.AddJsonFile($"ocelot.json", true, true);
configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

var logging = builder.Logging;

logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
logging.AddConsole();
logging.AddDebug();

var services = builder.Services;

services
    .AddOcelot()
    .AddCacheManager(settings =>
    {
        settings.WithDictionaryHandle();
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
