namespace FreeCourse.Services.Catalog.Model
{
    // one to one relation daki yerinden dolayı ayrı bir class
    public class Feature
    {
        public int Duration { get; set; }
        public bool IsDownload { get; set; } // download edilebilir mi edilemez mi ?
    }
}
