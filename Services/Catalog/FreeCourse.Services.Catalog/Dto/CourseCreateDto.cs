namespace FreeCourse.Services.Catalog.Dto
{
    /// <summary>
    /// coursedto dan create aşamasında ihtiyacımız olanları bırakıyoruz sadece
    /// </summary>
    public class CourseCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? UserId { get; set; }
        public string? Picture { get; set; }
        public FeatureDto? Feature { get; set; }
        public string? CategoryId { get; set; }
    }
}
