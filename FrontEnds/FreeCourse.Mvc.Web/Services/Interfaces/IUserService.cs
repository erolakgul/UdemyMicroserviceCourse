using FreeCourse.Mvc.Web.Models.ViewModel.Identity;

namespace FreeCourse.Mvc.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
