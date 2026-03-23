using FluentAssertions;
using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Features.Users.Commands.Register;
using HabitFlow.Domain.Repositories;
using HabitFlow.Domain.Users;
using NSubstitute;

namespace HabitFlow.UnitTests.ApplicationTests.Features.Users
{
    [TestFixture]
    public class RegisterUserCommandHandlerTests
    {
        private IUserRepository _userRepository = null!;
        private IAuthenticationService _authService = null!;
        private IUnitOfWork _unitOfWork = null!;
        private RegisterUserCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _authService = Substitute.For<IAuthenticationService>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _authService.HashPassword(Arg.Any<string>()).Returns("hashed_password");

            _handler = new RegisterUserCommandHandler(_userRepository, _authService, _unitOfWork);
        }

        private static RegisterUserCommand ValidCommand() =>
            new("joao@exemplo.com", "Senha@123", "Joao", "Silva");

        // ── Success ───────────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_NewEmail_CreatesUserAndReturnsSuccess()
        {
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            var result = await _handler.Handle(ValidCommand(), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Email.Should().Be("joao@exemplo.com");
            result.Value.UserId.Should().NotBeEmpty();
        }

        [Test]
        public async Task Handle_NewEmail_HashesPassword()
        {
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            await _handler.Handle(ValidCommand(), CancellationToken.None);

            _authService.Received(1).HashPassword("Senha@123");
        }

        [Test]
        public async Task Handle_NewEmail_AddsUserToRepository()
        {
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            await _handler.Handle(ValidCommand(), CancellationToken.None);
            await _userRepository.Received(1).AddAsync(
                Arg.Is<User>(u => u.Email == "joao@exemplo.com"),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Handle_NewEmail_SavesChanges()
        {
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            await _handler.Handle(ValidCommand(), CancellationToken.None);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        // ── Duplicate email ───────────────────────────────────────────────────────

        [Test]
        public async Task Handle_ExistingEmail_ReturnsFailure()
        {
            var existingUser = User.Create("joao@exemplo.com", "hash", "Joao", "Silva");
            _userRepository.GetByEmailAsync("joao@exemplo.com", Arg.Any<CancellationToken>())
                .Returns(existingUser);

            var result = await _handler.Handle(ValidCommand(), CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Email already registered");
        }

        [Test]
        public async Task Handle_ExistingEmail_DoesNotCallHashPassword()
        {
            var existingUser = User.Create("joao@exemplo.com", "hash", "Joao", "Silva");
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            await _handler.Handle(ValidCommand(), CancellationToken.None);

            _authService.DidNotReceive().HashPassword(Arg.Any<string>());
        }

        [Test]
        public async Task Handle_ExistingEmail_DoesNotSaveChanges()
        {
            var existingUser = User.Create("joao@exemplo.com", "hash", "Joao", "Silva");
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            await _handler.Handle(ValidCommand(), CancellationToken.None);
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
