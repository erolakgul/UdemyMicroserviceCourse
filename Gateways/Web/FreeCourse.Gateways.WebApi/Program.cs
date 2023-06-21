using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

#region ocelot service

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile($"Configuration.{builder.Environment.EnvironmentName}.json", true, true)
                            .Build();

builder.Services.AddOcelot(configuration);

#endregion



#region jwt sub : nameidentifier map lemesini kald�rmak i�in
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
#endregion

#region  authentication i�lemi i�in bir �ema belirliyoruz
builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationSchema",option =>
{
    option.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    option.Audience = "resource_gateway"; // identityserver config apiresources, token i�erisindeki bu bilgi sayesinde yetkili olup olmad���n� anlayaca��z
    option.RequireHttpsMetadata = false; // https kullanmad��m�z i�in kapal� yap�yoruz
});
#endregion

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

#region ocelot middleware
await app.UseOcelot();
#endregion

app.Run();
