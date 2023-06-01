using FreeCourse.Services.Discount.Models;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Discount.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<Response<List<Discounts>>> GetAllDiscountsAsync();
        Task<Response<Discounts>> GetDiscountsByIdAsync(int id);
        Task<Response<NoContent>> SaveAsync(Discounts discounts);
        Task<Response<NoContent>> UpdateAsync(Discounts discounts);
        Task<Response<NoContent>> DeleteAsync(int id);
        Task<Response<Discounts>> GetByCodeAndUserIdAsync(string code, string userId);
    }
}
