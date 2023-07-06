using FreeCourse.Mvc.Web.Models;
using FreeCourse.Shared.Dtos;
using IdentityModel.Client;

namespace FreeCourse.Mvc.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninInput signinInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
