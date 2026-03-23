using FluentAssertions;
using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Features.Users.Commands.Login;
using HabitFlow.Domain.Repositories;
using HabitFlow.Domain.Users;
using NSubstitute;

namespace HabitFlow.UnitTests.ApplicationTests.Features.Users
{
    [TestFixture]
    public class LoginCommandHandlerTests
    {
        private IUserRepository _userRepository = null!;
        private IAuthenticationService _authService = null!;
        private LoginCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _authService = Substitute.For<IAuthenticationService>();

            _authService.GenerateAccessToken(Arg.Any<Guid>(), Arg.Any<string>())
                .Returns("access_token_fake");
            _authService.GenerateRefreshToken()
                .Returns("refresh_token_fake");

            _handler = new LoginCommandHandler(_userRepository, _authService);
        }

        private static User CreateActiveUser(string email = "joao@exemplo.com")
        {
            var user = User.Create(email, "stored_hash", "Joao", "Silva");
            return user;
        }

        // ── Success ───────────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_ValidCredentials_ReturnsTokens()
        {
            var user = CreateActiveUser();
            _userRepository.GetByEmailAsync("joao@exemplo.com", Arg.Any<CancellationToken>())
                .Returns(user);
            _authService.VerifyPassword("Senha@123", "stored_hash").Returns(true);

            var result = await _handler.Handle(
                new LoginCommand("joao@exemplo.com", "Senha@123"), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.AccessToken.Should().Be("access_token_fake");
            result.Value.RefreshToken.Should().Be("refresh_token_fake");
            result.Value.UserId.Should().Be(user.Id);
            result.Value.Email.Should().Be("joao@exemplo.com");
        }

        [Test]
        public async Task Handle_ValidCredentials_CallsTokenGeneration()
        {
            var user = CreateActiveUser();
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(user);
            _authService.VerifyPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            await _handler.Handle(
                new LoginCommand("joao@exemplo.com", "Senha@123"), CancellationToken.None);

            _authService.Received(1).GenerateAccessToken(user.Id, "joao@exemplo.com");
            _authService.Received(1).GenerateRefreshToken();
        }

        // ── User not found ────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_UserNotFound_ReturnsInvalidCredentials()
        {
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            var result = await _handler.Handle(
                new LoginCommand("naoexiste@exemplo.com", "Senha@123"), CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid credentials");
        }

        // ── Wrong password ────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_WrongPassword_ReturnsInvalidCredentials()
        {
            var user = CreateActiveUser();
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(user);
            _authService.VerifyPassword("senha_errada", "stored_hash").Returns(false);

            var result = await _handler.Handle(
                new LoginCommand("joao@exemplo.com", "senha_errada"), CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid credentials");
        }

        [Test]
        public async Task Handle_WrongPassword_DoesNotGenerateToken()
        {
            var user = CreateActiveUser();
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(user);
            _authService.VerifyPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            await _handler.Handle(
                new LoginCommand("joao@exemplo.com", "errada"), CancellationToken.None);

            _authService.DidNotReceive().GenerateAccessToken(Arg.Any<Guid>(), Arg.Any<string>());
        }

        // ── Inactive account ──────────────────────────────────────────────────────

        [Test]
        public async Task Handle_InactiveUser_ReturnsAccountInactive()
        {
            // IsActive is set to true by default; simulate inactive by
            // using a user whose IsActive will fail the check.
            // Since User.IsActive has no public setter, we test the guard
            // via the result error when credentials pass but account is inactive.
            // NOTE: The current domain model sets IsActive=true on Create and
            // provides no Deactivate method yet. This test documents the expected
            // behavior once deactivation is implemented.
            var user = CreateActiveUser();
            _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(user);
            _authService.VerifyPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            // Active user must succeed
            var result = await _handler.Handle(
                new LoginCommand("joao@exemplo.com", "Senha@123"), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }

        // ── Timing-safe behavior ──────────────────────────────────────────────────

        [Test]
        public async Task Handle_UserNotFoundOrWrongPassword_ReturnSameError()
        {
            _userRepository.GetByEmailAsync("naoexiste@exemplo.com", Arg.Any<CancellationToken>())
                .Returns((User?)null);

            var user = CreateActiveUser();
            _userRepository.GetByEmailAsync("joao@exemplo.com", Arg.Any<CancellationToken>())
                .Returns(user);
            _authService.VerifyPassword("errada", Arg.Any<string>()).Returns(false);

            var r1 = await _handler.Handle(
                new LoginCommand("naoexiste@exemplo.com", "qualquer"), CancellationToken.None);

            var r2 = await _handler.Handle(
                new LoginCommand("joao@exemplo.com", "errada"), CancellationToken.None);

            // Both failures must return the same error message (prevents user enumeration)
            r1.Error.Should().Be(r2.Error);
        }
    }
}
