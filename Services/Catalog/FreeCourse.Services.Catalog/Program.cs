using FreeCourse.Services.Catalog.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

#region bu şekilde tanımlanırsa, herhangi bir yerde IOption<CustomDatabaseSettings> ile tüm veriler çekilebilir
builder.Services.Configure<CustomDatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
// devamında aşağıdaki şekilde tanımlarsak da ICustomDatabaseSettings i her çağırdığımızda
// otomatik olarak CustomDatabaseSettings i dolu olarak dönecek
builder.Services.AddSingleton<ICustomDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<CustomDatabaseSettings>>().Value;
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
