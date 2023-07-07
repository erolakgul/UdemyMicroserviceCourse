using FreeCourse.Mvc.Web.Models;
using FreeCourse.Mvc.Web.Models.Configurations;
using FreeCourse.Mvc.Web.Services.Interfaces;
using FreeCourse.Shared.Dtos;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace FreeCourse.Mvc.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _client;                         // api lere istek
        private readonly IHttpContextAccessor _httpContextAccessor;  // cookie lere erişim için
        private readonly ServiceApiSettings _serviceApiSettings; // identityserver a erişim bilgileri
        private readonly ClientSettings _clientSettings;         // client credential bilgilerine erişim

        public IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor, IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
        }

        public Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> SignIn(SigninInput signinInput)
        {
            throw new NotImplementedException();
        }
    }
}
