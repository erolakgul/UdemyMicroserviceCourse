namespace FreeCourse.Services.Catalog.Dto
{
    /// <summary>
    /// ıd ve name i client lardan gizlememize gerek yok, o yüzden onlar kalabilir
    /// </summary>
    public class CategoryDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
