using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

#region httpcontext, shared teki SharedIdentityService i kullanabilmek için
builder.Services.AddHttpContextAccessor();
#endregion

#region sharedclass tan kullanýcý id sini almak için di container a da ekliyoruz
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
#endregion

#region jwt remove sub : nameidentifier map lemesini kaldýrmak için
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
#endregion

#region  authentication iþlemi için bir þema belirliyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    option.Audience = "resource_discount"; // identityserver config apiresources, token içerisindeki bu bilgi sayesinde yetkili olup olmadýðýný anlayacaðýz
    option.RequireHttpsMetadata = false; // https kullanmadðýmýz için kapalý yapýyoruz
});
#endregion

#region authorize parametreleri, tüm controller larýn tepesinde authorize attribute ü çalýþmasý için
// burada global tanýmlamada, farklý olarak : authenticated olmuþ bir user gerekli diyoruz
var requiredAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
// eðer örneðin sadece read scope unu bekliyorsak aþaðýdaki gibi bir policy daha oluþturulur
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
