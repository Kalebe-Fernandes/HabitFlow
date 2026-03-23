using FluentAssertions;
using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Features.Habits.Commands.CreateHabit;
using HabitFlow.Domain.Habits;
using HabitFlow.Domain.Repositories;
using NSubstitute;

namespace HabitFlow.UnitTests.ApplicationTests.Features.Habits
{
    [TestFixture]
    public class CreateHabitCommandHandlerTests
    {
        private IHabitRepository _habitRepository = null!;
        private IUnitOfWork _unitOfWork = null!;
        private CreateHabitCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _habitRepository = Substitute.For<IHabitRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _handler = new CreateHabitCommandHandler(_habitRepository, _unitOfWork);
        }

        private static CreateHabitCommand DailyBinaryCommand(Guid? userId = null) =>
            new(userId ?? Guid.NewGuid(),
                "Meditar",
                "Meditacao diaria",
                "meditation",
                "#4CAF50",
                "Daily",
                "Binary",
                null,
                null,
                null,
                null,
                null,
                0);

        private static CreateHabitCommand WeeklyNumericCommand(Guid? userId = null) =>
            new(userId ?? Guid.NewGuid(),
                "Correr",
                "Corrida semanal",
                "run",
                "#FF5722",
                "Weekly",
                "Numeric",
                30m,
                "minutos",
                42, // Monday + Wednesday
                null,
                null,
                0);

        // ── Success – daily binary ────────────────────────────────────────────────

        [Test]
        public async Task Handle_DailyBinaryHabit_CreatesAndReturnsSuccess()
        {
            var command = DailyBinaryCommand();

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("Meditar");
            result.Value.HabitId.Should().NotBeEmpty();
        }

        [Test]
        public async Task Handle_DailyBinaryHabit_AddsHabitToRepository()
        {
            var command = DailyBinaryCommand();

            await _handler.Handle(command, CancellationToken.None);
            await _habitRepository.Received(1).AddAsync(
                Arg.Is<Habit>(h => h.Name == "Meditar"),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Handle_DailyBinaryHabit_SavesChanges()
        {
            await _handler.Handle(DailyBinaryCommand(), CancellationToken.None);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        // ── Success – weekly numeric ──────────────────────────────────────────────

        [Test]
        public async Task Handle_WeeklyNumericHabit_CreatesSuccessfully()
        {
            var command = WeeklyNumericCommand();

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("Correr");
        }

        // ── FrequencyType parsing ─────────────────────────────────────────────────

        [TestCase("Daily")]
        [TestCase("Weekly")]
        [TestCase("Custom")]
        public async Task Handle_ValidFrequencyType_Succeeds(string frequencyType)
        {
            var command = new CreateHabitCommand(
                Guid.NewGuid(), "Habito", null, "icon", "#000000",
                frequencyType, "Binary", null, null,
                frequencyType == "Daily" ? null : (int?)42,
                null, null, 0);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }

        // ── TargetType parsing ────────────────────────────────────────────────────

        [Test]
        public async Task Handle_NumericTargetWithValue_Succeeds()
        {
            var command = new CreateHabitCommand(
                Guid.NewGuid(), "Beber agua", null, "water", "#2196F3",
                "Daily", "Numeric", 8m, "copos",
                null, null, null, 0);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }

        // ── Domain validation propagation ─────────────────────────────────────────

        [Test]
        public async Task Handle_InvalidColorHex_ThrowsDomainValidationException()
        {
            var command = new CreateHabitCommand(
                Guid.NewGuid(), "Habito", null, "icon", "red",
                "Daily", "Binary", null, null, null, null, null, 0);

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>();
        }

        // ── User ownership propagated to habit ────────────────────────────────────

        [Test]
        public async Task Handle_CreatedHabit_BelongsToCommandUser()
        {
            var userId = Guid.NewGuid();
            var command = DailyBinaryCommand(userId);
            Habit? capturedHabit = null;

            await _habitRepository.AddAsync(
                Arg.Do<Habit>(h => capturedHabit = h),
                Arg.Any<CancellationToken>());

            await _handler.Handle(command, CancellationToken.None);

            capturedHabit.Should().NotBeNull();
            capturedHabit!.UserId.Should().Be(userId);
        }
    }
}
