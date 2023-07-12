using FreeCourse.Mvc.Web.Models.Configurations;
using Microsoft.Extensions.Options;

namespace FreeCourse.Mvc.Web.Services.Extentions
{
    public static class AppSettingsExtentions
    {
        public static IServiceCollection AddCustomAppSettingsConfigure(this IServiceCollection services,WebApplicationBuilder builder)
        {
            services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));
            services.AddScoped<ServiceApiSettings>(opt =>
            {
                var di = opt.GetRequiredService<IOptions<ServiceApiSettings>>();
                return di.Value;
            });

            services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
            services.AddScoped<ClientSettings>(opt =>
            {
                var di = opt.GetRequiredService<IOptions<ClientSettings>>();
                return di.Value;
            });
            return services;
        }
    }
}
