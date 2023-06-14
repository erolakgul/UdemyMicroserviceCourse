using FreeCourse.Services.Order.Infrastructure.Context;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region db context
builder.Services.AddDbContext<OrderDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
   configure =>
   {   // migration �n olu�aca�� library i belirtiyoruz
     configure.MigrationsAssembly("FreeCourse.Services.Order.Infrastructure");
   })
);
#endregion

#region https access
builder.Services.AddHttpContextAccessor();
#endregion

#region shared service
builder.Services.AddScoped<ISharedIdentityService,SharedIdentityService>();
#endregion

#region mediator service
builder.Services.AddMediatR(
       cfg => cfg.RegisterServicesFromAssembly(
                       typeof(FreeCourse.Services.Order.Application.Command.CreateOrderCommand).Assembly
                                              )
                           );
#endregion


#region jwt sub : nameidentifier map lemesini kald�rmak i�in
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
#endregion

#region  authentication i�lemi i�in bir �ema belirliyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    option.Audience = "resource_order"; // identityserver config apiresources, token i�erisindeki bu bilgi sayesinde yetkili olup olmad���n� anlayaca��z
    option.RequireHttpsMetadata = false; // https kullanmad��m�z i�in kapal� yap�yoruz
});
#endregion

#region authorize parametreleri, t�m controller lar�n tepesinde authorize attribute � �al��mas� i�in
// burada global tan�mlamada, farkl� olarak : authenticated olmu� bir user gerekli diyoruz
var requiredAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requiredAuthorizePolicy) { });
});
#endregion




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
