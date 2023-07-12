using Microsoft.AspNetCore.Authentication.Cookies;

namespace FreeCourse.Mvc.Web.Services.Extentions
{
    public static class AuthenticationExtentions
    {
        public static IServiceCollection AddCustomAuthConfigure(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
                 {
                     option.LoginPath = "/Auth/SignIn";
                     option.ExpireTimeSpan = TimeSpan.FromDays(2);
                     option.SlidingExpiration = true;
                     option.Cookie.Name = "udemywebcookie";
                 });
            return services;
        }
    }
}
