using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfContext.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(p => p.TransactionId).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18, 6)");
            builder.Property(p => p.Currency).IsRequired();
            builder.Property(p => p.Date).IsRequired();
            builder.Property(p => p.Status).IsRequired();
        }
    }
}
