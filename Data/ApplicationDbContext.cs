using Microsoft.EntityFrameworkCore;
using NewsAggregator.Models;

namespace NewsAggregator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Interests> Interests { get; set; }
        public DbSet<UserArticles> UserArticles { get; set; }
        public DbSet<SavedArticles> SavedArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Interests>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedArticles>()
                .HasKey(sa => new { sa.UserId, sa.ArticleId });

            modelBuilder.Entity<SavedArticles>()
                .HasOne(sa => sa.User) // One User
                .WithMany(u => u.SavedArticles) // Many SavedArticles
                .HasForeignKey(sa => sa.UserId) // Foreign key
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on User deletion

            modelBuilder.Entity<SavedArticles>()
                .HasOne(sa => sa.Articles) // One Article
                .WithMany(a => a.SavedArticles) // Many SavedArticles
                .HasForeignKey(sa => sa.ArticleId) // Foreign key
                .OnDelete(DeleteBehavior.Cascade);
        }



        
    }
}
