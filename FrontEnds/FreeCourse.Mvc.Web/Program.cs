using FreeCourse.Mvc.Web.Models.Configurations;
using FreeCourse.Mvc.Web.Resources;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region uygulama ayaða kalkarken settings dosyasýndaki karþýklarý class a aktaracak
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));
builder.Services.AddScoped<ServiceApiSettings>(opt =>
{
    var di = opt.GetRequiredService<IOptions<ServiceApiSettings>>();
    return di.Value;
});

builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.AddScoped<ClientSettings>(opt =>
{
    var di = opt.GetRequiredService<IOptions<ClientSettings>>();
    return di.Value;
});
#endregion

// Add services to the container.
//builder.Services.AddControllersWithViews();

#region localization datanotation
builder.Services.AddTransient<ISharedViewLocalizer, SharedViewLocalizer>();

builder.Services.AddControllersWithViews()
/*uses to make a change in Razor take effect immediately.*/
    .AddViewLocalization(options => options.ResourcesPath = "Resources")
    .AddDataAnnotationsLocalization(opts =>
    {
        opts.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName!);
            return factory.Create(nameof(SharedResource), assemblyName.Name!);
        };
    });
#endregion


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new List<CultureInfo> {
                                 new CultureInfo("en"),
                                  new CultureInfo("tr"),
                                 new CultureInfo("fr")
                              };
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

#region razor
var mvcBuilder = builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}
#endregion

//////////////////////////////////////////// middleware ///////////////////////////////////

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

#region localization
var getRequiredLocalizationOptionService = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(getRequiredLocalizationOptionService.Value); 
#endregion

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
