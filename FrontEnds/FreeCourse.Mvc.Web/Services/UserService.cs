using FreeCourse.Mvc.Web.Models.UriEnums;
using FreeCourse.Mvc.Web.Models.ViewModel.Identity;
using FreeCourse.Mvc.Web.Services.Interfaces;

namespace FreeCourse.Mvc.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
          return await _httpClient.GetFromJsonAsync<UserViewModel>(IdentityUriDefaults.api_user_getuser);
        }
    }
}
