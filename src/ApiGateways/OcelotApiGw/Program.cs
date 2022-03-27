using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var logging = builder.Logging;

logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
logging.AddConsole();
logging.AddDebug();

var services = builder.Services;
services.AddOcelot();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
