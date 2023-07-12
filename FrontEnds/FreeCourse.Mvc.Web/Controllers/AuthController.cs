using FreeCourse.Mvc.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Mvc.Web.Controllers
{
    public class AuthController : Controller
    {

        public AuthController()
        {
            
        }

        [HttpGet]   
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SigninInput signinInput)
        {
            return Ok(signinInput);
        }
    }
}
