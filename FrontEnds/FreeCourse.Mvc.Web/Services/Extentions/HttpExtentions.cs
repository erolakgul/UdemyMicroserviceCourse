using FreeCourse.Mvc.Web.Services.Interfaces;

namespace FreeCourse.Mvc.Web.Services.Extentions
{
    public static class HttpExtentions
    {

        public static IServiceCollection AddCustomHttpConfigure(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient<IIdentityService,IdentityService>();
            return services;
        }
    }
}
