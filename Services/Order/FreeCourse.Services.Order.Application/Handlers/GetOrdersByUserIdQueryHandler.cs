using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Application.Mapping;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Infrastructure.Context;
using FreeCourse.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FreeCourse.Services.Order.Application.Handlers
{
    /// <summary>
    /// IRequestHandler interface generic ilk class ında hangi query i görünce işlem yapacağını,
    /// ikinci generic class ında ise nasıl bir dönüş yapacağını bildiren class ı istiyor
    /// Handle isimli methodu da zorunlu olarak ezmemiz gerekiyor
    /// mediatR a GetOrdersByUserIdQuery bu sınıfı gönderdiğimde artık direkt olarak GetOrdersByUserIdQueryHandle 
    /// class ını çalıştıracak
    /// </summary>
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;
        // eğer repo ve service eri kullansaydık, burada IOrderService gibi bir interface i dependency i inject 
        // etmemiz gerekecekti, biz direkt olarak context üzerinden işlem yapacağımız için
        // bu şekilde ilerliyoruz
        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                                        .Include(x => x.OrderItems)  // lazy loading => çağırıldığında eklenir
                                        .Where(x => x.BuyerId == request.UserId).ToListAsync();

            if(!orders.Any())
            {   // siparişi yoksa yine de boş bir küme başarılı olarak dönsün
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }

            var orderDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return Response<List<OrderDto>>.Success(orderDto, 200);
        }
    }
}
