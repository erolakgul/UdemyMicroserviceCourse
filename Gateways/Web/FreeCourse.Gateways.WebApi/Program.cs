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



#region jwt sub : nameidentifier map lemesini kaldýrmak için
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
#endregion

#region  authentication iþlemi için bir þema belirliyoruz
builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationSchema",option =>
{
    option.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    option.Audience = "resource_gateway"; // identityserver config apiresources, token içerisindeki bu bilgi sayesinde yetkili olup olmadýðýný anlayacaðýz
    option.RequireHttpsMetadata = false; // https kullanmadðýmýz için kapalý yapýyoruz
});
#endregion

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

#region ocelot middleware
await app.UseOcelot();
#endregion

app.Run();
