namespace FreeCourse.Mvc.Web.Models.ViewModel.Catalog
{
    /// <summary>
    /// coursecreatedto dan
    /// </summary>
    public class CourseCreateInput
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? UserId { get; set; }
        public string? Picture { get; set; }
        public FeatureViewModel? Feature { get; set; }
        public string? CategoryId { get; set; }
    }
}
