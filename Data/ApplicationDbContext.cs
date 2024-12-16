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
        
            //modelBuilder.Entity<SavedArticles>()
            //    .HasOne(a => a.User)
            //    .WithMany()
            //    .HasForeignKey(a=>a.User.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<SavedArticles>()
            //    .HasOne(a => a.Articles)
            //    .WithMany()
            //    .HasForeignKey(a => a.ArticleId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }

        
    }
}
