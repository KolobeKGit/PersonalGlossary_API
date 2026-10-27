using Microsoft.EntityFrameworkCore;
using PersonalGlossary_API.Models;

namespace PersonalGlossary_API.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Term> Terms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Term>().HasData(
                new Term
                {
                    Id = 1,
                    Title = "Database",
                    Definition = "An organised collection of information",
                    CreatedDate = new DateTime(2026, 1, 1),
                    UpdatedDate = new DateTime(2026, 1, 1)
                },
                new Term
                {
                    Id = 2,
                    Title = "Driver",
                    Definition = "A program that acts as an interface between an application and hardware",
                    CreatedDate = new DateTime(2026, 2, 2),
                    UpdatedDate = new DateTime(2026, 2, 2)
                },
                new Term
                {
                    Id = 3,
                    Title = "Internet",
                    Definition = "A decentralized network of computers which communicate with one another",
                    CreatedDate = new DateTime(2026, 3, 3),
                    UpdatedDate = new DateTime(2026, 3, 3)
                },
                new Term
                {
                    Id = 4,
                    Title = "Network",
                    Definition = "Multiple computers and other devices connected together to share information",
                    CreatedDate = new DateTime(2026, 4, 4),
                    UpdatedDate = new DateTime(2026, 4, 4)
                }
                );
        }
    }
}
