using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Gamification.Events;

namespace HabitFlow.Domain.Gamification
{
    public sealed class UserLevel : AggregateRoot<Guid>
    {
        private readonly List<UserBadge> _badges = [];
        public int CurrentLevel { get; private set; }
        public long TotalXP { get; private set; }
        public int CurrentBalance { get; private set; }
        public IReadOnlyCollection<UserBadge> Badges => _badges.AsReadOnly();

        private UserLevel() { }

        public static UserLevel Create(Guid userId)
        {
            return new UserLevel
            {
                Id = userId,
                CurrentLevel = 1,
                TotalXP = 0,
                CurrentBalance = 0
            };
        }

        public void AwardXP(int xpAmount, string reason, Guid? relatedEntityId = null)
        {
            if (xpAmount <= 0) throw new ValidationException(nameof(xpAmount), "XP amount must be positive");

            TotalXP += xpAmount;
            var newLevel = CalculateLevel(TotalXP);

            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new XPAwardedEvent(Id, xpAmount, TotalXP, reason, relatedEntityId));

            if (newLevel > CurrentLevel)
            {
                CurrentLevel = newLevel;
                AddDomainEvent(new LevelUpEvent(Id, CurrentLevel));
            }
        }

        public void AwardCurrency(int amount, string reason)
        {
            if (amount <= 0) throw new ValidationException(nameof(amount), "Amount must be positive");
            CurrentBalance += amount;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new CurrencyEarnedEvent(Id, amount, CurrentBalance, reason));
        }

        public void SpendCurrency(int amount, string reason)
        {
            if (amount <= 0) throw new ValidationException(nameof(amount), "Amount must be positive");
            if (CurrentBalance < amount) throw new DomainException("Insufficient balance", "INSUFFICIENT_BALANCE");
            CurrentBalance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AwardBadge(int badgeId, string badgeName)
        {
            if (_badges.Any(b => b.BadgeId == badgeId)) return;
            _badges.Add(new UserBadge { UserId = Id, BadgeId = badgeId, EarnedAt = DateTime.UtcNow });
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new BadgeEarnedEvent(Id, badgeId, badgeName));
        }

        private static int CalculateLevel(long totalXP)
        {
            // Formula: XP_Required(N) = 100 × N²
            var level = 1;
            while (CalculateXPForLevel(level + 1) <= totalXP) level++;
            return level;
        }

        private static long CalculateXPForLevel(int level) => 100L * level * level;
    }
}
