using EfContext.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EfContext
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.Date)
                .IsClustered(false);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.Status)
                .IsClustered(false);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.Currency)
                .IsClustered(false);
        }
    }
}
