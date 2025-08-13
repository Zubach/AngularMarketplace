using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AngularMarketplace.Server
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Producer> Producers { get; set; }

      

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Producer>()
                .HasMany(x => x.Categories)
                .WithMany(x => x.Producers)
                .UsingEntity<ProducerCategory>(
                    r => r.HasOne<ProductCategory>().WithMany().HasForeignKey(x => x.CategoryId),
                    l => l.HasOne<Producer>().WithMany().HasForeignKey(x => x.ProducerId)
                );
            base.OnModelCreating(builder);
        }

    }
}
