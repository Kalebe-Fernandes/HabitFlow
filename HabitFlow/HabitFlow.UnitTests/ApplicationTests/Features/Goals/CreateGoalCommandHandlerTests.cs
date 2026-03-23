using FluentAssertions;
using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Features.Goals.Commands.CreateGoal;
using HabitFlow.Domain.Goals;
using HabitFlow.Domain.Repositories;
using NSubstitute;

namespace HabitFlow.UnitTests.ApplicationTests.Features.Goals
{
    [TestFixture]
    public class CreateGoalCommandHandlerTests
    {
        private IGoalRepository _goalRepository = null!;
        private IUnitOfWork _unitOfWork = null!;
        private CreateGoalCommandHandler _handler = null!;

        private static readonly Guid _userId = Guid.NewGuid();
        private static readonly DateTime _start = DateTime.UtcNow.Date;
        private static readonly DateTime _target = _start.AddMonths(3);

        [SetUp]
        public void SetUp()
        {
            _goalRepository = Substitute.For<IGoalRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new CreateGoalCommandHandler(_goalRepository, _unitOfWork);
        }

        private static CreateGoalCommand ValidCommand(Guid? userId = null) =>
            new(userId ?? _userId,
                "Ler 12 livros",
                "Meta anual de leitura",
                12m,
                "livros",
                _start,
                _target);

        // ── Success ───────────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_ValidCommand_ReturnsSuccess()
        {
            var result = await _handler.Handle(ValidCommand(), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("Ler 12 livros");
            result.Value.GoalId.Should().NotBeEmpty();
        }

        [Test]
        public async Task Handle_ValidCommand_AddsGoalToRepository()
        {
            await _handler.Handle(ValidCommand(), CancellationToken.None);
            await _goalRepository.Received(1).AddAsync(
                Arg.Is<Goal>(g => g.Name == "Ler 12 livros" && g.UserId == _userId),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Handle_ValidCommand_SavesChanges()
        {
            await _handler.Handle(ValidCommand(), CancellationToken.None);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Handle_ValidCommand_GoalBelongsToCorrectUser()
        {
            var specificUser = Guid.NewGuid();
            Goal? capturedGoal = null;

            await _goalRepository.AddAsync(Arg.Do<Goal>(g => capturedGoal = g),Arg.Any<CancellationToken>());
            await _handler.Handle(ValidCommand(specificUser), CancellationToken.None);

            capturedGoal.Should().NotBeNull();
            capturedGoal!.UserId.Should().Be(specificUser);
        }

        // ── Domain validation propagation ─────────────────────────────────────────

        [Test]
        public async Task Handle_EmptyName_ThrowsDomainValidationException()
        {
            var command = new CreateGoalCommand(_userId, "", null, 10m, "itens", _start, _target);
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("*Goal name is required*");
        }

        [Test]
        public async Task Handle_TargetDateBeforeStartDate_ThrowsDomainValidationException()
        {
            var command = new CreateGoalCommand(_userId, "Meta", null, 10m, "itens", _target, _start);
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>();
        }

        [TestCase(0)]
        [TestCase(-5)]
        public async Task Handle_ZeroOrNegativeTargetValue_ThrowsDomainValidationException(decimal value)
        {
            var command = new CreateGoalCommand(_userId, "Meta", null, value, "itens", _start, _target);
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>();
        }

        // ── Does not save on domain error ─────────────────────────────────────────

        [Test]
        public async Task Handle_InvalidCommand_DoesNotSaveChanges()
        {
            var command = new CreateGoalCommand(_userId, "", null, 10m, "itens", _start, _target);

            try { await _handler.Handle(command, CancellationToken.None); } catch { }

            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
