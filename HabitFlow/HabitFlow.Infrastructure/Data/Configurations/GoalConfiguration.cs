using HabitFlow.Domain.Goals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class GoalConfiguration : IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> builder)
        {
            builder.ToTable("Goals", "goals");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.Description)
                .HasMaxLength(1000);

            builder.Property(g => g.TargetValue)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(g => g.TargetUnit)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.CurrentValue)
                .HasPrecision(18, 2);

            builder.Property(g => g.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasIndex(g => g.UserId);
            builder.HasIndex(g => new { g.UserId, g.Status });

            builder.Property(g => g.CreatedAt).IsRequired();
            builder.Property(g => g.UpdatedAt).IsRequired();

            builder.Ignore(g => g.DomainEvents);
        }
    }
}
