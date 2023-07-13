using FreeCourse.Mvc.Web.Models.Poco;

namespace FreeCourse.Mvc.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
