using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Goals
{
    public sealed class GoalHabit : Entity<long>
    {
        public Guid GoalId { get; set; }
        public Guid HabitId { get; set; }
        public decimal ContributionWeight { get; set; } = 1.0m;
    }
}
