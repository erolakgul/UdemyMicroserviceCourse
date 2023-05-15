using FreeCourse.Services.Catalog.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FreeCourse.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IOptions<CustomDatabaseSettings> _options;
        private ICustomDatabaseSettings _customDatabaseSettings;
        public HomeController(IOptions<CustomDatabaseSettings> options, ICustomDatabaseSettings customDatabaseSettings)
        {
            _options = options;
            _customDatabaseSettings = customDatabaseSettings;
        }

        [HttpGet]
        public string Get()
        {
            return _customDatabaseSettings.DatabaseName;//_options.Value.DatabaseName;
        }
    }
}
