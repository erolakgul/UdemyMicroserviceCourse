using FreeCourse.Mvc.Web.Handlers;
using FreeCourse.Mvc.Web.Models.Configurations;
using FreeCourse.Mvc.Web.Services;
using FreeCourse.Mvc.Web.Services.Interfaces;

namespace FreeCourse.Mvc.Web.Extentions
{
    public static class HttpExtentions
    {

        public static IServiceCollection AddCustomHttpConfigure(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // httpcontextaccessor ı kullanabilmek için service di ya ekliyoruz
            services.AddHttpContextAccessor();
            // service lere handler eklenir
            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();

            // settings dosyasındaki veri class nesnesine dönüştürülür
            var serviceApiSettings = builder.Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            // IIdentityService altında kullanılar httpclient nesnesinin parametreleri buradan değiştirilir
            services.AddHttpClient<IIdentityService, IdentityService>(opt =>
            {

            });

            // IUserService interface i altında kullanılan httpclient nesnesinin verileri burada değiştirilir
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            // ICatalogService interface i altında kullanılan httpclient nesnesinin verileri burada değiştirilir
            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                //// gateway api yönlendirmeyi catalog api ye yapacak
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}"); 
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            return services;
        }
    }
}
