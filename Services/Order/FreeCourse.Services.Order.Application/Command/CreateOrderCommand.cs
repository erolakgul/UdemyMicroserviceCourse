using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Shared.Dtos;
using MediatR;

namespace FreeCourse.Services.Order.Application.Command
{
    public class CreateOrderCommand : IRequest<Response<CreatedOrderDto>>
    {
        //satın alan id si
        public string? BuyerId { get; set; }
        //Sipariş/sepet içindeki item lar
        public List<OrderItemDto>? OrderItems { get; set; }
        //siparişin gideceği adres bilgisi
        public AddressDto? AddressDto { get; set; }
    }
}
