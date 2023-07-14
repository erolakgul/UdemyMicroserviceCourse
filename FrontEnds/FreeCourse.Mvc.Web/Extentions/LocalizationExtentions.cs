using FreeCourse.Mvc.Web.Resources;
using System.Globalization;
using System.Reflection;

namespace FreeCourse.Mvc.Web.Extentions
{
    public static class LocalizationExtentions
    {
        public static IServiceCollection AddCustomLocalizationConfigure(this IServiceCollection services)
        {
            services.AddTransient<ISharedViewLocalizer, SharedViewLocalizer>();

            services.AddControllersWithViews()
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


            services.Configure<RequestLocalizationOptions>(options =>
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

            return services;
        }
    }
}
