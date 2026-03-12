using HabitFlow.Domain.Habits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class HabitCompletionConfiguration : IEntityTypeConfiguration<HabitCompletion>
    {
        public void Configure(EntityTypeBuilder<HabitCompletion> builder)
        {
            builder.ToTable("HabitCompletions", "habits");

            builder.HasKey(hc => hc.Id);

            // Explicit IDENTITY so EF Core populates Id after insert
            builder.Property(hc => hc.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(hc => hc.CompletionDate).IsRequired();

            builder.Property(hc => hc.CompletedValue).HasPrecision(18, 2);

            builder.Property(hc => hc.Notes).HasMaxLength(500);

            builder.Property(hc => hc.CreatedAt).IsRequired();

            // Unique: one completion per habit per day
            builder.HasIndex(hc => new { hc.HabitId, hc.CompletionDate }).IsUnique();
            builder.HasIndex(hc => hc.HabitId);
            builder.HasIndex(hc => hc.CompletionDate);
        }
    }
}
