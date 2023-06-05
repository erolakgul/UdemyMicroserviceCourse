using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

#region httpcontext, shared teki SharedIdentityService i kullanabilmek i�in
builder.Services.AddHttpContextAccessor();
#endregion

#region sharedclass tan kullan�c� id sini almak i�in di container a da ekliyoruz
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
#endregion

#region jwt remove sub : nameidentifier map lemesini kald�rmak i�in
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
#endregion

#region  authentication i�lemi i�in bir �ema belirliyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    option.Audience = "resource_discount"; // identityserver config apiresources, token i�erisindeki bu bilgi sayesinde yetkili olup olmad���n� anlayaca��z
    option.RequireHttpsMetadata = false; // https kullanmad��m�z i�in kapal� yap�yoruz
});
#endregion

#region authorize parametreleri, t�m controller lar�n tepesinde authorize attribute � �al��mas� i�in
// burada global tan�mlamada, farkl� olarak : authenticated olmu� bir user gerekli diyoruz
var requiredAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
// e�er �rne�in sadece read scope unu bekliyorsak a�a��daki gibi bir policy daha olu�turulur
var readRequiredAuthorizePolicy = new AuthorizationPolicyBuilder()
                                   .RequireClaim("scope", "discount_read").Build();

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requiredAuthorizePolicy) { });
    //opt.Filters.Add(new AuthorizeFilter(readRequiredAuthorizePolicy) { });
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
