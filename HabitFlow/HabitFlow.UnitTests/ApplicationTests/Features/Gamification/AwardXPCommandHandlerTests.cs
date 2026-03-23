using FluentAssertions;
using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Features.Gamification.Commands.AwardXP;
using HabitFlow.Domain.Gamification;
using HabitFlow.Domain.Repositories;
using NSubstitute;

namespace HabitFlow.UnitTests.ApplicationTests.Features.Gamification
{
    [TestFixture]
    public class AwardXPCommandHandlerTests
    {
        private IUserLevelRepository _userLevelRepository = null!;
        private IUnitOfWork _unitOfWork = null!;
        private AwardXPCommandHandler _handler = null!;

        private static readonly Guid _userId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _userLevelRepository = Substitute.For<IUserLevelRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new AwardXPCommandHandler(_userLevelRepository, _unitOfWork);
        }

        private static AwardXPCommand ValidCommand(int amount = 50, Guid? userId = null) => new(userId ?? _userId, amount, "habit_completed");

        // ── Existing user level ───────────────────────────────────────────────────

        [Test]
        public async Task Handle_ExistingUserLevel_AwardsXP()
        {
            var userLevel = UserLevel.Create(_userId);
            _userLevelRepository.GetByUserIdAsync(_userId, Arg.Any<CancellationToken>()).Returns(userLevel);

            var result = await _handler.Handle(ValidCommand(50), CancellationToken.None);
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalXP.Should().Be(50);
        }

        [Test]
        public async Task Handle_ExistingUserLevel_ReturnsCorrectLevel()
        {
            var userLevel = UserLevel.Create(_userId);
            _userLevelRepository.GetByUserIdAsync(_userId, Arg.Any<CancellationToken>()).Returns(userLevel);

            var result = await _handler.Handle(ValidCommand(50), CancellationToken.None);
            result.Value.CurrentLevel.Should().Be(1);
            result.Value.LeveledUp.Should().BeFalse();
        }

        [Test]
        public async Task Handle_XPEnoughForLevelUp_ReportsLeveledUp()
        {
            var userLevel = UserLevel.Create(_userId);
            _userLevelRepository.GetByUserIdAsync(_userId, Arg.Any<CancellationToken>()).Returns(userLevel);

            // 400 XP → level 2 (formula: 100 * 2² = 400)
            var result = await _handler.Handle(ValidCommand(400), CancellationToken.None);

            result.Value.LeveledUp.Should().BeTrue();
            result.Value.CurrentLevel.Should().Be(2);
        }

        [Test]
        public async Task Handle_ExistingUserLevel_SavesChanges()
        {
            var userLevel = UserLevel.Create(_userId);
            _userLevelRepository.GetByUserIdAsync(_userId, Arg.Any<CancellationToken>()).Returns(userLevel);

            await _handler.Handle(ValidCommand(), CancellationToken.None);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        // ── No user level yet (first time) ────────────────────────────────────────

        [Test]
        public async Task Handle_NoUserLevel_CreatesNewUserLevel()
        {
            _userLevelRepository.GetByUserIdAsync(_userId, Arg.Any<CancellationToken>()).Returns((UserLevel?)null);
            var result = await _handler.Handle(ValidCommand(30), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.TotalXP.Should().Be(30);
        }

        [Test]
        public async Task Handle_NoUserLevel_AddsToRepository()
        {
            _userLevelRepository.GetByUserIdAsync(_userId, Arg.Any<CancellationToken>())
                .Returns((UserLevel?)null);

            await _handler.Handle(ValidCommand(), CancellationToken.None);
            await _userLevelRepository.Received(1).AddAsync(Arg.Any<UserLevel>(), Arg.Any<CancellationToken>());
        }

        // ── RelatedEntityId propagation ───────────────────────────────────────────

        [Test]
        public async Task Handle_WithRelatedEntityId_SucceedsNormally()
        {
            var userLevel = UserLevel.Create(_userId);
            _userLevelRepository.GetByUserIdAsync(_userId, Arg.Any<CancellationToken>()).Returns(userLevel);

            var command = new AwardXPCommand(_userId, 20, "habit_completed", Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
