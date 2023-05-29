using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region  authentication iþlemi için bir þema belirliyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    option.Audience = "photostock_catalog"; // identityserver config apiresources, token içerisindeki bu bilgi sayesinde yetkili olup olmadýðýný anlayacaðýz
    option.RequireHttpsMetadata = false; // https kullanmadðýmýz için kapalý yapýyoruz
});
#endregion

// Add services to the container.
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter()); // tüm controller lara authorize attribute ünü ekle
});

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

// photos klasörüne eriþim ayarý
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
