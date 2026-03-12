using HabitFlow.Domain.Gamification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class CurrencyTransactionConfiguration : IEntityTypeConfiguration<CurrencyTransaction>
    {
        public void Configure(EntityTypeBuilder<CurrencyTransaction> builder)
        {
            builder.ToTable("CurrencyTransactions", "gamification");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(c => c.UserId).IsRequired();

            builder.Property(c => c.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.Amount).IsRequired();

            builder.Property(c => c.BalanceAfter).IsRequired();

            builder.Property(c => c.Reason)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(c => c.CreatedAt).IsRequired();

            builder.HasIndex(c => c.UserId);
            builder.HasIndex(c => c.CreatedAt);
        }
    }
}
