using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    /// <summary>
    /// ef core features
    /// owned entity type
    /// shadow properties
    /// backing fields
    /// </summary>
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreateDate { get; private set; }
        //owned entity type
        public Address? Address { get;private set; }
        public string? BuyerId { get;private set; } // property e userid gönderilecek

        private readonly List<OrderItem> _orderItems; // backing field ef core kendi dolduracak
        //sadece okuma işlemi yapan OrderItems verilerini döner
        public IReadOnlyCollection<OrderItem> OrderItems { get {  return _orderItems; } }
        // boş constructure
        public Order() { }

        public Order(Address? address, string? buyerId)
        {
            _orderItems = new();
            CreateDate = DateTime.Now;

            Address = address;
            BuyerId = buyerId;
        }

        public void AddOrderItem(string? productId, string? productName, string? pictureUrl, decimal price, int quantity = 1)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);

            if (!existProduct)
            {
                _orderItems.Add(new OrderItem(productId, productName, pictureUrl, price, quantity) { });
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
    }
}
