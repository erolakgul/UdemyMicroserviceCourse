using FreeCourse.Services.Discount.Models;
using FreeCourse.Services.Discount.Services.Interfaces;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Discount.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountController : CustomBaseController
    {

        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAllDiscountsAsync());
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var discount = await _discountService.GetDiscountsByIdAsync(Id);

            return CreateActionResultInstance(discount);
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var discount = await _discountService.GetByCodeAndUserIdAsync(code, _sharedIdentityService.GetUserId);

            return CreateActionResultInstance(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Discounts discount)
        {
            return CreateActionResultInstance(await _discountService.SaveAsync(discount));
        }


        [HttpPut]
        public async Task<IActionResult> Update(Discounts discount)
        {
            return CreateActionResultInstance(await _discountService.UpdateAsync(discount));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.DeleteAsync(id));
        }
    }
}
