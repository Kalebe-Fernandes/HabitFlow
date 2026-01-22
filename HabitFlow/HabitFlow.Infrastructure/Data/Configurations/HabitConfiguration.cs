using HabitFlow.Domain.Habits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class HabitConfiguration : IEntityTypeConfiguration<Habit>
    {
        public void Configure(EntityTypeBuilder<Habit> builder)
        {
            builder.ToTable("Habits", "habits");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(h => h.Description)
                .HasMaxLength(1000);

            builder.Property(h => h.IconName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(h => h.ColorHex)
                .IsRequired()
                .HasMaxLength(7);

            builder.Property(h => h.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasIndex(h => h.UserId);
            builder.HasIndex(h => new { h.UserId, h.Status });

            // Owned types for Frequency and Target
            builder.OwnsOne(h => h.Frequency, frequency =>
            {
                frequency.Property(f => f.Type)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .HasColumnName("FrequencyType");

                frequency.Property(f => f.DaysOfWeek)
                    .HasColumnName("DaysOfWeekFrequency");
            });

            builder.OwnsOne(h => h.Target, target =>
            {
                target.Property(t => t.Type)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .HasColumnName("TargetType");

                target.Property(t => t.Value)
                    .HasColumnName("TargetValue")
                    .HasPrecision(18, 2);

                target.Property(t => t.Unit)
                    .HasMaxLength(50)
                    .HasColumnName("TargetUnit");
            });

            // Audit fields
            builder.Property(h => h.CreatedAt).IsRequired();
            builder.Property(h => h.UpdatedAt).IsRequired();

            builder.Ignore(h => h.DomainEvents);
        }
    }
}
