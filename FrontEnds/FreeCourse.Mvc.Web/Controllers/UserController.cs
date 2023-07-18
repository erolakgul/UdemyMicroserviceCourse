using FreeCourse.Mvc.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Mvc.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            // IUserService in kullanıldığı bu an da ResourceOwnerPasswordTokenHandler devreye girecek ve token ı 
            return View(await _userService.GetUser());
        }
    }
}
