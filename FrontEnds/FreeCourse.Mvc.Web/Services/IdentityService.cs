using FreeCourse.Mvc.Web.Models;
using FreeCourse.Mvc.Web.Models.Configurations;
using FreeCourse.Mvc.Web.Services.Interfaces;
using FreeCourse.Shared.Dtos;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NuGet.Common;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace FreeCourse.Mvc.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;                         // api lere istek
        private readonly IHttpContextAccessor _httpContextAccessor;  // cookie lere erişim için
        private readonly ServiceApiSettings _serviceApiSettings;     // identityserver a erişim bilgileri
        private readonly ClientSettings _clientSettings;             // client credential bilgilerine erişim

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor,
            IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshTokenAsync()
        {
            #region identityserver daki tüm endpointleri çek
            //IdentityModel paketinde extention olarak tanımlı olduğu için GetDiscoveryDocumentAsync methodu httpclient için gelir
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(
                   new DiscoveryDocumentRequest
                   {
                       Address = _serviceApiSettings.BaseUri,
                       Policy = new DiscoveryPolicy { RequireHttps = false } // https ları kapattığımız için
                   }
                );
            // discovery altına userinfo gibi token endpoint gibi url ler geliyor olacak 
            if (discovery.IsError)
            {
                throw discovery.Exception; // hata varsa fırlat
            }
            #endregion

            #region refreshtoken cookie den okunur ve yeni bir token almak için kullanılır
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(
                      OpenIdConnectParameterNames.RefreshToken
                                                        );
            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebResourceOwner.ClientId,
                ClientSecret = _clientSettings.WebResourceOwner.ClientSecret,
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint
            };

            // refresh token da yeni bir token alıyoruz
            var newtoken = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (newtoken.IsError)
            {
                return null;
            }

            // herşey ok ise
            //authentication token bilgisini de yeniliyoruz
            var authenticationNewToken =
                           new List<AuthenticationToken>()
                           {
                               // openId protokolünü kullanacağız
                               new AuthenticationToken {
                                                        Name = OpenIdConnectParameterNames.AccessToken
                                                        , Value = newtoken.AccessToken
                                                       }
                               ,  new AuthenticationToken {
                                                        Name = OpenIdConnectParameterNames.RefreshToken
                                                        , Value = newtoken.RefreshToken
                                                       }
                               ,  new AuthenticationToken {
                                                        Name = OpenIdConnectParameterNames.ExpiresIn
                                                        , Value = DateTime.Now.AddSeconds(newtoken.ExpiresIn)
                                                        .ToString("o",CultureInfo.InvariantCulture) // herhangi bir culture  bilgisine bağımlı olmadan
                                                       }
                           };

            // var olan authenticated bilgilerini çekiyoruz
            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            // token bilgisini değiştiriyoruz
            var properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationNewToken);
            // ve cookie yi güncelliyoruz
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                authenticationResult.Principal, properties);

            #endregion

            return newtoken;
        }

        public async Task RevokeRefreshTokenAsync()
        {
            #region identityserver daki tüm endpointleri çek
            //IdentityModel paketinde extention olarak tanımlı olduğu için GetDiscoveryDocumentAsync methodu httpclient için gelir
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(
                   new DiscoveryDocumentRequest
                   {
                       Address = _serviceApiSettings.BaseUri,
                       Policy = new DiscoveryPolicy { RequireHttps = false } // https ları kapattığımız için
                   }
                );
            // discovery altına userinfo gibi token endpoint gibi url ler geliyor olacak 
            if (discovery.IsError)
            {
                throw discovery.Exception; // hata varsa fırlat
            }
            #endregion


            #region cookie den  refresh token okunur ve revoke işlemi gerçekleştirilir
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebResourceOwner.ClientId,
                ClientSecret = _clientSettings.WebResourceOwner.ClientSecret,
                Address = discovery.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
            #endregion

            throw new NotImplementedException();
        }

        public async Task<Response<bool>> SignInAsync(SigninInput signinInput)
        {
            #region identityserver daki tüm endpointleri çek
            //IdentityModel paketinde extention olarak tanımlı olduğu için GetDiscoveryDocumentAsync methodu httpclient için gelir
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(
                   new DiscoveryDocumentRequest
                   {
                       Address = _serviceApiSettings.BaseUri,
                       Policy = new DiscoveryPolicy { RequireHttps = false } // https ları kapattığımız için
                   }
                );
            // discovery altına userinfo gibi token endpoint gibi url ler geliyor olacak 
            if (discovery.IsError)
            {
                throw discovery.Exception; // hata varsa fırlat
            }
            #endregion

            #region resourceownerpassword için modeli oluşturup, token ı işliyoruz

            var passwordTokenRequest = new PasswordTokenRequest  //ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.WebResourceOwner.ClientId,
                ClientSecret = _clientSettings.WebResourceOwner.ClientSecret,
                UserName = signinInput.Email,
                Password = signinInput.Password,
                Address = discovery.TokenEndpoint  // tüm endpointler içerisinden token için olan ile set liyoruz.
            };
            // httpclient extention ı IDentityModel paketi ile
            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
            if (token.IsError)
            {
                // IdentityServer da IdentityResourceOwnerPasswordValidator isimli method ta
                // ekstra custom hataları da eklemiştik biz en son
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                // onları, shared lib e eklediğimiz errordto nesnesi üzerinden geri okuyacağız
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // property isimleri küçük/büyük işlemine takılma
                });

                return Response<bool>.Fail(errorDto.Errors, 400);
            }
            #endregion


            #region userinfo bilgileri alınır

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = discovery.UserInfoEndpoint, // bu sefer de discovery den userinfo ya istek yap
            };
            // yine bir httpclient extention ı IDentityModel paketi ile
            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            #endregion

            #region claim leri cookie lere kaydetme işlemi
            // uygulamaya username i ve rolleri claim de nereden okuyacağınız söylüyoruz (name,role)
            // eğer claimlerde username i örneğin kullaniciadi gibi bir claimproperty üzerinden gönderirsek
            // name yerine kullaniciadi yazmamız gerekecek
            ClaimsIdentity claimsIdentity = new(userInfo.Claims,
                                                CookieAuthenticationDefaults.AuthenticationScheme
                                                , "name", "role");
            // claim ler için bir kimlik oluştuuruyoruz şimdi, cookie oluşturmak için bu kimlik önemli
            ClaimsPrincipal claimPrincipal = new(claimsIdentity);

            //acces ile refresh token ı da cookie içerisinde tutacağız.
            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(
                           new List<AuthenticationToken>()
                           {
                               // openId protokolünü kullanacağız
                               new AuthenticationToken {
                                                        Name = OpenIdConnectParameterNames.AccessToken
                                                        , Value = token.AccessToken
                                                       }
                               ,  new AuthenticationToken {
                                                        Name = OpenIdConnectParameterNames.RefreshToken
                                                        , Value = token.RefreshToken
                                                       }
                               ,  new AuthenticationToken {
                                                        Name = OpenIdConnectParameterNames.ExpiresIn
                                                        , Value = DateTime.Now.AddSeconds(token.ExpiresIn)
                                                        .ToString("o",CultureInfo.InvariantCulture) // herhangi bir culture  bilgisine bağımlı olmadan
                                                       }
                           }
                        );

            // session bazlı mı yoksa bir ömrü olacak mı cookie nin bunu client tan alıyoruz.
            authenticationProperties.IsPersistent = signinInput.IsRemember;

            #endregion

            #region login olma işlemi

            await _httpContextAccessor.HttpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 claimPrincipal, authenticationProperties
                );

            #endregion

            return Response<bool>.Success(200);
        }
    }
}
