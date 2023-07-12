using FreeCourse.Mvc.Web.Models;
using FreeCourse.Mvc.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Mvc.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;
        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]   
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SigninInput signinInput)
        {
            #region null check
            ArgumentNullException.ThrowIfNull(signinInput); 
            #endregion

            var response = await _identityService.SignInAsync(signinInput);

            if (response == null || !response.IsSucces)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(String.Empty, x);
                });
                return View();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
