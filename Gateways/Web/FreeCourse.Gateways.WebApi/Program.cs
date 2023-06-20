using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region ocelot service

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile($"Configuration.{builder.Environment.EnvironmentName}.json", true, true)
                            .Build();

builder.Services.AddOcelot(configuration);

#endregion

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

#region ocelot middleware
await app.UseOcelot();
#endregion

app.Run();
