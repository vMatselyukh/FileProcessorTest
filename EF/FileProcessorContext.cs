using Microsoft.EntityFrameworkCore;

namespace EF
{
    public class FileProcessorContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public FileProcessorContext(DbContextOptions options) : base(options)
        {
        }

        public FileProcessorContext() : this(new DbContextOptions<FileProcessorContext>())
        {
        }
    }
}
