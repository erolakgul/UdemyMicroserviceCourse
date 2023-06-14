using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Payment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : CustomBaseController
    {

        public PaymentController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> ReceivedPayment()
        {
            await Task.Delay(1000);

            return CreateActionResultInstance(Response<NoContent>.Success(200));
        }
    }
}
