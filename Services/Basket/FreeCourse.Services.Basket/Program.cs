using FreeCourse.Services.Basket.Services;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

#region httpcontext, shared teki SharedIdentityService i kullanabilmek için
builder.Services.AddHttpContextAccessor();
#endregion

#region sharedclass tan kullanýcý id sini almak için di container a da ekliyoruz
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
#endregion

#region configure appsettings
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
#endregion

#region redis option pattern, uygulama çalýþýr çalýþmaz baðlantý kurmasý için
builder.Services.AddSingleton<RedisService>(sp =>
{
    var redisService = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisService.Host,redisService.Port);
    redis.Connect();
    return redis;
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

app.UseAuthorization();

app.MapControllers();

app.Run();
