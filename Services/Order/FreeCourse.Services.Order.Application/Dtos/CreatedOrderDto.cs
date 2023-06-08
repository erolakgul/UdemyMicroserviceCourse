namespace FreeCourse.Services.Order.Application.Dtos
{
    /// <summary>
    /// sipariş oluştuktan sonra sadece orderid numarasını döneceğiz
    /// </summary>
    public class CreatedOrderDto
    {
        public int OrderId { get; set; }
    }
}
