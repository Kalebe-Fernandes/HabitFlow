using FluentAssertions;
using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Gamification;
using HabitFlow.Domain.Gamification.Events;

namespace HabitFlow.UnitTests.Domain.Gamification
{
    [TestFixture]
    public class UserLevelTests
    {
        private static readonly Guid _userId = Guid.NewGuid();

        private static UserLevel CreateUserLevel() => UserLevel.Create(_userId);

        // ── Create ────────────────────────────────────────────────────────────────

        [Test]
        public void Create_SetsInitialState()
        {
            var userLevel = CreateUserLevel();

            userLevel.Id.Should().Be(_userId);
            userLevel.CurrentLevel.Should().Be(1);
            userLevel.TotalXP.Should().Be(0);
            userLevel.CurrentBalance.Should().Be(0);
            userLevel.Badges.Should().BeEmpty();
        }

        // ── AwardXP ───────────────────────────────────────────────────────────────

        [Test]
        public void AwardXP_ValidAmount_IncreasesTotalXP()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardXP(100, "habit_completed");

            userLevel.TotalXP.Should().Be(100);
        }

        [Test]
        public void AwardXP_ValidAmount_RaisesXPAwardedEvent()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardXP(50, "habit_completed");

            userLevel.DomainEvents.Should().Contain(e => e is XPAwardedEvent);
            var evt = (XPAwardedEvent)userLevel.DomainEvents
                .Single(e => e is XPAwardedEvent);
            evt.UserId.Should().Be(_userId);
            evt.Amount.Should().Be(50);
            evt.TotalXP.Should().Be(50);
        }

        [Test]
        public void AwardXP_EnoughForLevelUp_IncrementsLevel()
        {
            var userLevel = CreateUserLevel();

            // Level 1 → 2 requires 100*2²=400 XP; award 400 to trigger
            userLevel.AwardXP(400, "test");

            userLevel.CurrentLevel.Should().Be(2);
        }

        [Test]
        public void AwardXP_LevelUp_RaisesLevelUpEvent()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardXP(400, "test");

            userLevel.DomainEvents.Should().Contain(e => e is LevelUpEvent);
            var evt = (LevelUpEvent)userLevel.DomainEvents.Single(e => e is LevelUpEvent);
            evt.UserId.Should().Be(_userId);
            evt.NewLevel.Should().Be(2);
        }

        [Test]
        public void AwardXP_NotEnoughForLevelUp_LevelStaysTheSame()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardXP(50, "test");

            userLevel.CurrentLevel.Should().Be(1);
            userLevel.DomainEvents.Should().NotContain(e => e is LevelUpEvent);
        }

        [Test]
        public void AwardXP_MultipleAwards_AccumulatesCorrectly()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardXP(100, "a");
            userLevel.AwardXP(200, "b");
            userLevel.AwardXP(100, "c");

            userLevel.TotalXP.Should().Be(400);
            userLevel.CurrentLevel.Should().Be(2);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-100)]
        public void AwardXP_ZeroOrNegativeAmount_ThrowsValidationException(int amount)
        {
            var userLevel = CreateUserLevel();

            var act = () => userLevel.AwardXP(amount, "test");

            act.Should().Throw<ValidationException>()
                .WithMessage("XP amount must be positive");
        }

        // ── AwardCurrency ─────────────────────────────────────────────────────────

        [Test]
        public void AwardCurrency_ValidAmount_IncreasesBalance()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardCurrency(50, "daily_bonus");

            userLevel.CurrentBalance.Should().Be(50);
        }

        [Test]
        public void AwardCurrency_RaisesCurrencyEarnedEvent()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardCurrency(100, "daily_bonus");

            userLevel.DomainEvents.Should().Contain(e => e is CurrencyEarnedEvent);
            var evt = (CurrencyEarnedEvent)userLevel.DomainEvents
                .Single(e => e is CurrencyEarnedEvent);
            evt.Amount.Should().Be(100);
            evt.NewBalance.Should().Be(100);
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void AwardCurrency_ZeroOrNegative_ThrowsValidationException(int amount)
        {
            var userLevel = CreateUserLevel();

            var act = () => userLevel.AwardCurrency(amount, "bonus");

            act.Should().Throw<ValidationException>()
                .WithMessage("Amount must be positive");
        }

        // ── SpendCurrency ─────────────────────────────────────────────────────────

        [Test]
        public void SpendCurrency_SufficientBalance_DecreasesBalance()
        {
            var userLevel = CreateUserLevel();
            userLevel.AwardCurrency(100, "bonus");

            userLevel.SpendCurrency(40, "item_purchase");

            userLevel.CurrentBalance.Should().Be(60);
        }

        [Test]
        public void SpendCurrency_ExactBalance_SetsBalanceToZero()
        {
            var userLevel = CreateUserLevel();
            userLevel.AwardCurrency(100, "bonus");

            userLevel.SpendCurrency(100, "item_purchase");

            userLevel.CurrentBalance.Should().Be(0);
        }

        [Test]
        public void SpendCurrency_InsufficientBalance_ThrowsDomainException()
        {
            var userLevel = CreateUserLevel();
            userLevel.AwardCurrency(50, "bonus");

            var act = () => userLevel.SpendCurrency(100, "item_purchase");

            act.Should().Throw<DomainException>()
                .WithMessage("Insufficient balance");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void SpendCurrency_ZeroOrNegative_ThrowsValidationException(int amount)
        {
            var userLevel = CreateUserLevel();

            var act = () => userLevel.SpendCurrency(amount, "item_purchase");

            act.Should().Throw<ValidationException>();
        }

        // ── AwardBadge ────────────────────────────────────────────────────────────

        [Test]
        public void AwardBadge_NewBadge_AddsToBadgesCollection()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardBadge(1, "Iniciante");

            userLevel.Badges.Should().HaveCount(1);
        }

        [Test]
        public void AwardBadge_NewBadge_RaisesBadgeEarnedEvent()
        {
            var userLevel = CreateUserLevel();

            userLevel.AwardBadge(1, "Iniciante");

            userLevel.DomainEvents.Should().Contain(e => e is BadgeEarnedEvent);
            var evt = (BadgeEarnedEvent)userLevel.DomainEvents.Single(e => e is BadgeEarnedEvent);
            evt.BadgeId.Should().Be(1);
            evt.BadgeName.Should().Be("Iniciante");
        }

        [Test]
        public void AwardBadge_DuplicateBadge_IsIdempotent()
        {
            var userLevel = CreateUserLevel();
            userLevel.AwardBadge(1, "Iniciante");
            userLevel.ClearDomainEvents();

            userLevel.AwardBadge(1, "Iniciante");

            userLevel.Badges.Should().HaveCount(1);
            userLevel.DomainEvents.Should().NotContain(e => e is BadgeEarnedEvent);
        }

        // ── Level formula ─────────────────────────────────────────────────────────

        [Test]
        public void AwardXP_LevelProgression_FollowsFormula()
        {
            var userLevel = CreateUserLevel();
            // 100 * 2^2 = 400 XP → level 2
            // 100 * 3^2 = 900 XP → level 3
            // 100 * 4^2 = 1600 XP → level 4

            userLevel.AwardXP(1600, "test");

            userLevel.CurrentLevel.Should().Be(4);
            userLevel.TotalXP.Should().Be(1600);
        }
    }
}
