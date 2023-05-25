using FreeCourse.IdentityServer.Dto;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            var appUser = new ApplicationUser { UserName = signUpDto.UserName, Email = signUpDto.Email , City = signUpDto.City };

            var result = await _userManager.CreateAsync(appUser,signUpDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(Response<NoContent>.
                                   Fail(result.Errors.Select(x => x.Description).ToList(),400)
                                 );
            }

            return NoContent();
        }
    }
}
