using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Shared.Dtos;
using MediatR;

namespace FreeCourse.Services.Order.Application.Queries
{
    /// <summary>
    /// bu class ım geriye Response<List<OrderDto>> verisini döneceği için IRequest interface ime ekledim
    /// </summary>
    public class GetOrdersByUserIdQuery : IRequest<Response<List<OrderDto>>>
    {
        public string? UserId { get; set; } // parametre olarak gönderilecekleri property olarak ekliyoruz

    }
}
