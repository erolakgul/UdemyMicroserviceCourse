namespace FreeCourse.Services.Order.Application.Dtos
{
    public class AddressDto
    {
        public string? Province { get; private set; } // şehit
        public string? District { get; private set; } // ilçe
        public string? Street { get; private set; }  // cadde sokak
        public string? ZipCode { get; private set; } // posta kodu
        public string? Line { get; private set; }
    }
}
