using FreeCourse.Services.Order.Application.Command;
using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure.Context;
using FreeCourse.Shared.Dtos;
using MediatR;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderCommandHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // yeni adres bilgisi
            var newAddress = new Address(request.AddressDto.Province, 
                                         request.AddressDto.District, 
                                         request.AddressDto.Street, 
                                         request.AddressDto.ZipCode, 
                                         request.AddressDto.Line);
            // yeni sipariş bilgisi
            var newOrder = new Domain.OrderAggregate.Order(newAddress, request.BuyerId);
            // yeni sipariş/sepet item larının yeni sipariş içine doldurulması
            request.OrderItems.ForEach(x =>
            {   // orderitems readonly idi, bu işlem için oluşturduğumuz addorderitem custom method kullanıldı
                newOrder.AddOrderItem(x.ProductId,x.ProductName,x.PictureUrl,x.Price,x.Quantity);
            });

            await _orderDbContext.AddAsync(newOrder);

            await _orderDbContext.SaveChangesAsync();

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto() { OrderId = newOrder.Id},204);
        }
    }
}
