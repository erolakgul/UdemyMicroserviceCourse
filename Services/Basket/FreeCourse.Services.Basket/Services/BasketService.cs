using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System.Text.Json;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<bool>> DeleteAsync(string? userId)
        {
            var status = await _redisService.GetDatabase().KeyDeleteAsync(userId);

            return status ? Response<bool>.Success(status, 204) : Response<bool>.Fail("Basket not found", 404);
        }

        public async Task<Response<BasketHeadDto>> GetBasketByUserIdAsync(string? userId)
        {
            var existBasket = await _redisService.GetDatabase().StringGetAsync(userId);

            if (String.IsNullOrEmpty(existBasket))
            {
                return Response<BasketHeadDto>.Fail("User does not have any item",400);
            }

            return Response<BasketHeadDto>.Success(JsonSerializer.Deserialize<BasketHeadDto>(existBasket),200);
        }

        public async Task<Response<bool>> SaveOrUpdateAsync(BasketHeadDto basketHeadDto)
        {
            var status = await _redisService.GetDatabase().StringSetAsync(basketHeadDto.UserId
                                                           , JsonSerializer.Serialize<BasketHeadDto>(basketHeadDto));

            return status ? Response<bool>.Success(status, 204) : Response<bool>.Fail("Basket could not save or update",500);
        }
    }
}
