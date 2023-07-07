using FreeCourse.Mvc.Web.Models;
using FreeCourse.Mvc.Web.Models.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace FreeCourse.Mvc.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        public HomeController(ILogger<HomeController> logger, ServiceApiSettings serviceApiSettings, 
             ClientSettings clientSettings)
        {
            _logger = logger;
            _serviceApiSettings = serviceApiSettings;
            _clientSettings = clientSettings;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SigninInput signinInput)
        {
            return Ok(signinInput);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}