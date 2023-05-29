using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region  authentication i�lemi i�in bir �ema belirliyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    option.Audience = "photostock_catalog"; // identityserver config apiresources, token i�erisindeki bu bilgi sayesinde yetkili olup olmad���n� anlayaca��z
    option.RequireHttpsMetadata = false; // https kullanmad��m�z i�in kapal� yap�yoruz
});
#endregion

// Add services to the container.
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter()); // t�m controller lara authorize attribute �n� ekle
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

// photos klas�r�ne eri�im ayar�
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
