using Microsoft.EntityFrameworkCore;
using petmanagement.Models;

namespace petmanagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Pet entity
            modelBuilder.Entity<Pet>(entity =>
            {
                entity.HasKey(e => e.PetID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Breed).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.ImagePath).HasMaxLength(500);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure AdoptionRequest entity
            modelBuilder.Entity<AdoptionRequest>(entity =>
            {
                entity.HasKey(e => e.RequestID);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(20);

                // Configure relationships
                entity.HasOne(d => d.User)
                    .WithMany(p => p.AdoptionRequests)
                    .HasForeignKey(d => d.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.AdoptionRequests)
                    .HasForeignKey(d => d.PetID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Name = "Admin",
                    Email = "admin@petadoption.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin",
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}