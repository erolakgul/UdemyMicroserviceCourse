namespace FreeCourse.Services.Discount.Models
{
    [Dapper.Contrib.Extensions.Table("discounts")]
    public class Discounts
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int Rate { get; set; }    // discount rate
        public string? Code { get; set; } // discount code
        public DateTime ValidDate { get; set; } // last valid date for code

    }
}
