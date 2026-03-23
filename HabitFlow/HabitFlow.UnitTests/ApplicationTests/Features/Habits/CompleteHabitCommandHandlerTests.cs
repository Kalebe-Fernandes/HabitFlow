using FluentAssertions;
using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Features.Habits.Commands.CompleteHabit;
using HabitFlow.Domain.Habits;
using HabitFlow.Domain.Habits.ValueObjects;
using HabitFlow.Domain.Repositories;
using NSubstitute;

namespace HabitFlow.UnitTests.ApplicationTests.Features.Habits
{
    [TestFixture]
    public class CompleteHabitCommandHandlerTests
    {
        private IHabitRepository _habitRepository = null!;
        private IUnitOfWork _unitOfWork = null!;
        private CompleteHabitCommandHandler _handler = null!;

        private static readonly Guid _userId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _habitRepository = Substitute.For<IHabitRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new CompleteHabitCommandHandler(_habitRepository, _unitOfWork);
        }

        private static Habit CreateHabit(Guid? userId = null)
        {
            return Habit.Create(
                userId ?? _userId,
                "Meditar",
                null,
                "meditation",
                "#4CAF50",
                HabitFrequency.Daily(),
                HabitTarget.Binary());
        }

        private static CompleteHabitCommand ValidCommand(Guid habitId, Guid? userId = null) =>
            new(habitId, userId ?? _userId, DateTime.Today, null, null, null, null);

        // ── Success ───────────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccess()
        {
            var habit = CreateHabit();
            _habitRepository.GetByIdWithCompletionsAsync(habit.Id, Arg.Any<CancellationToken>())
                .Returns(habit);

            var result = await _handler.Handle(ValidCommand(habit.Id), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.CurrentStreak.Should().Be(1);
            result.Value.XPAwarded.Should().Be(habit.XPPerCompletion);
        }

        [Test]
        public async Task Handle_ValidRequest_SavesChanges()
        {
            var habit = CreateHabit();
            _habitRepository.GetByIdWithCompletionsAsync(habit.Id, Arg.Any<CancellationToken>())
                .Returns(habit);

            await _handler.Handle(ValidCommand(habit.Id), CancellationToken.None);

            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        // ── Not found ─────────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_HabitNotFound_ReturnsFailure()
        {
            var unknownId = Guid.NewGuid();
            _habitRepository.GetByIdWithCompletionsAsync(unknownId, Arg.Any<CancellationToken>())
                .Returns((Habit?)null);

            var result = await _handler.Handle(ValidCommand(unknownId), CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Habit not found");
        }

        [Test]
        public async Task Handle_HabitNotFound_DoesNotSaveChanges()
        {
            _habitRepository.GetByIdWithCompletionsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((Habit?)null);

            await _handler.Handle(ValidCommand(Guid.NewGuid()), CancellationToken.None);

            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        // ── Authorization ─────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_HabitBelongsToOtherUser_ReturnsUnauthorized()
        {
            var otherUser = Guid.NewGuid();
            var habit = CreateHabit(otherUser);
            _habitRepository.GetByIdWithCompletionsAsync(habit.Id, Arg.Any<CancellationToken>())
                .Returns(habit);

            // Command uses _userId, habit belongs to otherUser
            var result = await _handler.Handle(ValidCommand(habit.Id, _userId), CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Unauthorized");
        }

        // ── Duplicate completion propagation ──────────────────────────────────────

        [Test]
        public async Task Handle_DuplicateCompletion_PropagatesDomainException()
        {
            var habit = CreateHabit();
            var date = DateOnly.FromDateTime(DateTime.Today);
            habit.Complete(date);

            _habitRepository.GetByIdWithCompletionsAsync(habit.Id, Arg.Any<CancellationToken>()).Returns(habit);

            var command = new CompleteHabitCommand(habit.Id, _userId, DateTime.Today, null, null, null, null);
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*already completed*");
        }

        // ── Optional fields ───────────────────────────────────────────────────────

        [Test]
        public async Task Handle_WithNumericValueAndNotes_Succeeds()
        {
            var habit = Habit.Create(_userId, "Correr", null, "run", "#FF5722",
                HabitFrequency.Daily(), HabitTarget.Numeric(30m, "minutos"));

            _habitRepository.GetByIdWithCompletionsAsync(habit.Id, Arg.Any<CancellationToken>())
                .Returns(habit);

            var command = new CompleteHabitCommand(
                habit.Id, _userId, DateTime.Today, 35m, "Otima corrida", 4, 5);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
