using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FreeCourse.Services.Basket.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }


        [HttpGet]
        public async Task<IActionResult> GetBasketAsync()
        {
            // jwt den user id yi zaten alacağız, o yüzden parametre olarak string userıd olmasına gerek yok
            return CreateActionResultInstance(
                                      await _basketService.GetBasketByUserIdAsync(_sharedIdentityService.GetUserId)
                                      );
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateAsync(BasketHeadDto basketHeadDto)
        {
            var response = await _basketService.SaveOrUpdateAsync(basketHeadDto);

            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            return CreateActionResultInstance(
                                      await _basketService.DeleteAsync(_sharedIdentityService.GetUserId)
                                      );
        }

    }
}
