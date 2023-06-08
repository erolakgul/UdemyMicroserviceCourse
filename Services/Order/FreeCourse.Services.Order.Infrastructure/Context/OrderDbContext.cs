using Microsoft.EntityFrameworkCore;

namespace FreeCourse.Services.Order.Infrastructure.Context
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";
        //yapıcı method bir option alır ve bu option ı da base ine göndeririz
        public OrderDbContext(DbContextOptions<OrderDbContext> dbContextOptions):base(dbContextOptions)
        {
        }
        // 2 tane db nesnemizi tanıtıyoruz
        public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region tablo şema ve isim ayarı
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);
            #endregion

            #region tablo kolon tipleri için varsa özel ayarlar
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>()
                                      .Property(x => x.Price).HasColumnType("decimal(18,2)");//, den sonra 2 karakter 
            #endregion

            #region tablolar arası ilişkilerdeki özel durumlar
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(o => o.Address).WithOwner();
            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }
}
