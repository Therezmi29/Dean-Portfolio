using Dean_Resume.Models;
using Microsoft.EntityFrameworkCore;

namespace Dean_Resume.Data
{
    public class DeanContext : DbContext
    {
        public DeanContext(DbContextOptions<DeanContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ContactMe> ContactMe { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region seedData
            modelBuilder.Entity<User>().HasData(
                new User() { UserId=1 ,Email = "Hesamsa77@gmail.com", Password = "1234" });

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
