using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketHeadDto>> GetBasketByUserIdAsync(string? userId);
        Task<Response<bool>> SaveOrUpdateAsync(BasketHeadDto basketHeadDto);
        Task<Response<bool>> DeleteAsync(string? userId);
    }
}
