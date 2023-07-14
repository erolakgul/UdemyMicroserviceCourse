using FreeCourse.Mvc.Web.Exceptions;
using FreeCourse.Mvc.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using System.Net.Http.Headers;

namespace FreeCourse.Mvc.Web.Handlers
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        // cookie leri okumak için inject ediyoruz
        private readonly IHttpContextAccessor _contextAccessor;
        //refresh token ı alabilmek için inject ediyoruz
        private readonly IIdentityService _identityService;
        //loglama yapmak için
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor contextAccessor, 
              IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger)
        {
            _contextAccessor = contextAccessor;
            _identityService = identityService;
            _logger = logger;
        }

        // her istek başlatıldığında bu method araya girmekte. biz de kontrollerimizi burada yapacağız.
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            #region accesstoken okunur
            var accessToken = await _contextAccessor.HttpContext
                                                    .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            #endregion

            #region request in header ına cookie deki token ı set ediyoruz
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            #endregion

            #region request sonucunun statüsünü görmek istiyoruz
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized) 
            {
                // 401 alınıyorsa, refreshtoken üzerinden yeni token alınır
                var refreshTokenResponse = await _identityService.GetAccessTokenByRefreshTokenAsync();
                if(refreshTokenResponse != null)
                {
                    #region request in header ına refreshtoken ile yeniden alınan token ı set ediyoruz
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshTokenResponse.AccessToken);
                    #endregion

                    // request tekrar gönderiliyor, response u yine incelenecek
                    response = await base.SendAsync(request, cancellationToken);

                }
            }

            #endregion


            #region eğer hala response 401 ise

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // hata fırlatılacak artık, bu hatayı da middleware de yakalayıp, redirecttoaction yapacağız
                throw new UnAuthorizeException();
            }
            #endregion

            return response;
            //return base.SendAsync(request, cancellationToken);
        }
    }
}
