using FreeCourse.Mvc.Web.Services.Extentions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

#region http context
builder.Services.AddCustomHttpConfigure();
#endregion

#region uygulama aya�a kalkarken settings dosyas�ndaki kar��klar� class a aktaracak
builder.Services.AddCustomAppSettingsConfigure(builder);
#endregion

#region localization datanotation
builder.Services.AddCustomLocalizationConfigure();
#endregion

#region razor
var mvcBuilder = builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    // development a�amas�nda razor da yap�lan de�i�iklikler direkt olarak ekrana yans�mas� i�in
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
