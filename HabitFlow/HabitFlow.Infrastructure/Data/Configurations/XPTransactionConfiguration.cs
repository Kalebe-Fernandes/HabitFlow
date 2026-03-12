using HabitFlow.Domain.Gamification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class XPTransactionConfiguration : IEntityTypeConfiguration<XPTransaction>
    {
        public void Configure(EntityTypeBuilder<XPTransaction> builder)
        {
            builder.ToTable("XPTransactions", "gamification");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.Amount).IsRequired();

            builder.Property(x => x.TotalXPAfter).IsRequired();

            builder.Property(x => x.LevelAfter).IsRequired();

            builder.Property(x => x.Reason)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
