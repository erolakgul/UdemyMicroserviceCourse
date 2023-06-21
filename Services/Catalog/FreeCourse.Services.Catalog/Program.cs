using FreeCourse.Services.Catalog.Dto;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Services.Interfaces;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region custom services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();
#endregion

#region  authentication işlemi için bir şema belirliyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    option.Audience = "credential_catalog"; // identityserver config apiresources, token içerisindeki bu bilgi sayesinde yetkili olup olmadığını anlayacağız
    option.RequireHttpsMetadata = false; // https kullanmadğımız için kapalı yapıyoruz
});
#endregion


#region authorize parametreleri, tüm controller ların tepesinde authorize attribute ü çalışması için
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter() { });
});
#endregion


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


/***************************************************************************************/
var app = builder.Build();


// using işlemi bittikten sonra memory den düşmesi için scope açıyoruz
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var categoryService = serviceProvider.GetRequiredService<ICategoryService>();

    if (!categoryService.GetAllAsync().Result.Data.Any())
    {
        categoryService.CreateAsync(new CategoryCreateDto() { Name = "Aspdotnet Core Kursu"}).Wait();
        categoryService.CreateAsync(new CategoryCreateDto() { Name = "Sql Server Kursu" }).Wait();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.MapGet("{area:exists}/{controller=Home}/{action=Index}/{id?}",() => "eee");
//app.MapGet("/", () => "This is an endpoint created in ASP.NET 6");

app.Run();
