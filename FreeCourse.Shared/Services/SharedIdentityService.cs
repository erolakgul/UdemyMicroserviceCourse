using Microsoft.AspNetCore.Http;

namespace FreeCourse.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private HttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(HttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// context içindeki user a ait token ın claim lerinden sub verisini döndür
        /// </summary>
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
