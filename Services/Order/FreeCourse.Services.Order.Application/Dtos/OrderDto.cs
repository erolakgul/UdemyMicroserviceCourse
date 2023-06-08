namespace FreeCourse.Services.Order.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreateDate { get;  set; }
        //owned entity type
        public AddressDto? Address { get;  set; }
        public string? BuyerId { get;  set; } // property e userid gönderilecek
        public List<OrderItemDto>? OrderItems { get;  set; }
    }
}
