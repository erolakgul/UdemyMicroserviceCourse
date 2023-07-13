using FreeCourse.Mvc.Web.Services.Extentions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

#region http context
builder.Services.AddCustomHttpConfigure(builder);
#endregion

#region uygulama ayaða kalkarken settings dosyasýndaki karþýklarý class a aktaracak
builder.Services.AddCustomAppSettingsConfigure(builder);
#endregion

#region localization datanotation
builder.Services.AddCustomLocalizationConfigure();
#endregion

#region view
builder.Services.AddControllersWithViews();
#endregion

#region authentication
builder.Services.AddCustomAuthConfigure();
#endregion

#region razor
var mvcBuilder = builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    // development aþamasýnda razor da yapýlan deðiþiklikler direkt olarak ekrana yansýmasý için
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


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
