namespace FreeCourse.Services.Order.Application.Dtos
{
    public class AddressDto
    {
        public string? Province { get;  set; } // şehit
        public string? District { get;  set; } // ilçe
        public string? Street { get;  set; }  // cadde sokak
        public string? ZipCode { get;  set; } // posta kodu
        public string? Line { get;  set; }
    }
}
