using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class OrderItem : Entity
    {
        public string? ProductId { get;private set; }
        public string? ProductName { get; private set; }
        public string? PictureUrl { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; } = 1;

        public OrderItem(){}
        public OrderItem(string? productId, string? productName, string? pictureUrl, decimal price, int quantity = 1)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
            Quantity = quantity;
        }

        public void UpdateOrderItem(string? productName, string? pictureUrl, decimal price)
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

    }
}
