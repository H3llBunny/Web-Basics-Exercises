using Microsoft.EntityFrameworkCore;
using TweetExercise.Data.Models;

namespace TweetExercise.Data
{
    public class TweetDbContext : DbContext
    {
        public TweetDbContext()
        {
        }

        public TweetDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tweet> Tweets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=TweetDb;Integrated Security=true;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
