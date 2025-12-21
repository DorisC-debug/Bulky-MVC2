using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RazorProyect_Temp.Models;

namespace RazorProyect_Temp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id= 1, DisplayOrder= 1, Name="Love"},
                 new Category { Id = 2, DisplayOrder = 2, Name = "Fantasy" },
                  new Category { Id = 3, DisplayOrder = 3, Name = "Horror" }
                );
        }

    }
}
