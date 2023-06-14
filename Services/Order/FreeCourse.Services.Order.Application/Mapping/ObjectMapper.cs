using AutoMapper;

namespace FreeCourse.Services.Order.Application.Mapping
{
    public static class ObjectMapper
    {
        //normalde static class lar uygulama ayağa kalkar kalmaz init olur
        //ama sadece çağırıldığı anda init olması isteniyorsa lazy class ı kullanılır
        private static readonly Lazy<IMapper> mapper = new (() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomMapping>(); // ustom mapping class ımızı profil olarak ekliyoruz
            });

            return config.CreateMapper();// geriye bir IMapper dönüyoruz
        });

        // dışarıdan erişilebilen bu property ile talep edene IMapper nesnesini dönmüş olacağız.
        public static IMapper Mapper => mapper.Value;
    }
}
