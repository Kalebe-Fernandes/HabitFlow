using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Gamification
{
    public sealed class UserBadge : Entity<long>
    {
        public Guid UserId { get; private set; }
        public int BadgeId { get; private set; }
        public DateTime EarnedAt { get; private set; }

        private UserBadge() { }

        /// <summary>
        /// Creates a new UserBadge record. Called exclusively by UserLevel.AwardBadge().
        /// </summary>
        public static UserBadge Create(Guid userId, int badgeId)
        {
            return new UserBadge
            {
                UserId = userId,
                BadgeId = badgeId,
                EarnedAt = DateTime.UtcNow
            };
        }
    }
}
