using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Gamification
{
    public sealed class UserBadge : Entity<long>
    {
        public Guid UserId { get; set; }
        public int BadgeId { get; set; }
        public DateTime EarnedAt { get; set; }
    }
}
