using Microsoft.EntityFrameworkCore;
using WebApiPractice.Models;

namespace WebApiPractice.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options) { }
     
        public DbSet<Shirt> Shirts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Shirt>().HasData(
                new Shirt { ShirtId = 1, Brand = "My Brand", Color = "Black", Gender = "Male", Price = 1000, Size = 8 },
            new Shirt { ShirtId = 2, Brand = "My Brand", Color = "Blue", Gender = "female", Price = 800, Size = 7 },
            new Shirt { ShirtId = 3, Brand = "Your Brand", Color = "White", Gender = "Male", Price = 90, Size = 10 },
            new Shirt { ShirtId = 4, Brand = "Your Brand", Color = "Yellow", Gender = "female", Price = 400, Size = 9 }

                );

            ;
        }
    }
}
