using HabitFlow.Domain.Goals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class GoalHabitConfiguration : IEntityTypeConfiguration<GoalHabit>
    {
        public void Configure(EntityTypeBuilder<GoalHabit> builder)
        {
            builder.ToTable("GoalHabits", "goals");

            builder.HasKey(gh => gh.Id);

            builder.Property(gh => gh.ContributionWeight)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.HasIndex(gh => new { gh.GoalId, gh.HabitId })
                .IsUnique();

            builder.HasIndex(gh => gh.GoalId);
            builder.HasIndex(gh => gh.HabitId);
        }
    }
}
