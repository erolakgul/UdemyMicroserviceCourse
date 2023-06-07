using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    //[Owned]
    public class Address : ValueObject
    {
        public string? Province { get; private set; } // şehit
        public string? District { get; private set; } // ilçe
        public string? Street { get; private set; }  // cadde sokak
        public string? ZipCode { get; private set; } // posta kodu
        public string? Line { get; private set; }

        //private string? _name;
        //public string? Name { 
        //                      get { return _name; } 
        //                      set { 
        //                            if(value?.Length < 50)
        //                              {
        //                                _name = value;
        //                              }
        //                              else { throw new ArgumentOutOfRangeException("value bigger than 50 charackter!"); }
        //                          } 
        //                    }

        public Address(string? province, string? district, string? street, string? zipCode, string? line)
        {
            Province = province;
            District = district;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }

        public void SetZipCode(string zipcode)
        {
            // bussiness rule : gelen zipcode değerinin hepsini upper la ve L yerine A yazdır
            ZipCode = zipcode.ToString().ToUpper().Replace("L","A");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Province;
            yield return District;
            yield return Street;
            yield return ZipCode;
            yield return Line;
        }
    }
}
