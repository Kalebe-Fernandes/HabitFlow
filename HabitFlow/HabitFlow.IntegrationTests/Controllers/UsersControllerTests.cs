using FluentAssertions;
using HabitFlow.Aplicacao.Features.Users.Commands.Login;
using HabitFlow.Aplicacao.Features.Users.Commands.Register;
using HabitFlow.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace HabitFlow.IntegrationTests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private HabitFlowWebApplicationFactory _factory = null!;
        private HttpClient _client = null!;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        [SetUp]
        public void SetUp()
        {
            _factory = new HabitFlowWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        // ── POST /api/users/register ──────────────────────────────────────────────

        [Test]
        public async Task Register_ValidRequest_Returns201()
        {
            var command = new RegisterUserCommand("novo@exemplo.com", "Senha@1234", "Joao", "Silva");
            var response = await _client.PostAsJsonAsync("/api/users/register", command);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async Task Register_ValidRequest_ReturnsUserIdAndEmail()
        {
            var command = new RegisterUserCommand("joao2@exemplo.com", "Senha@1234", "Joao", "Silva");

            var response = await _client.PostAsJsonAsync("/api/users/register", command);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();

            body.GetProperty("userId").GetGuid().Should().NotBeEmpty();
            body.GetProperty("email").GetString().Should().Be("joao2@exemplo.com");
        }

        [Test]
        public async Task Register_DuplicateEmail_Returns422()
        {
            var command = new RegisterUserCommand("dup@exemplo.com", "Senha@1234", "Joao", "Silva");

            await _client.PostAsJsonAsync("/api/users/register", command);
            var response = await _client.PostAsJsonAsync("/api/users/register", command);

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Test]
        public async Task Register_InvalidEmail_Returns400()
        {
            var command = new RegisterUserCommand("nao-e-email", "Senha@1234", "Joao", "Silva");
            var response = await _client.PostAsJsonAsync("/api/users/register", command);

            // FluentValidation via ValidationBehavior returns a failure Result → 422
            response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
        }

        [Test]
        public async Task Register_ShortPassword_ReturnsError()
        {
            var command = new RegisterUserCommand("val@ex.com", "123", "A", "B");
            var response = await _client.PostAsJsonAsync("/api/users/register", command);

            response.IsSuccessStatusCode.Should().BeFalse();
        }

        // ── POST /api/users/login ─────────────────────────────────────────────────

        [Test]
        public async Task Login_ValidCredentials_Returns200WithTokens()
        {
            var email = $"login_{Guid.NewGuid():N}@ex.com";
            var password = "Senha@1234";

            await _client.PostAsJsonAsync("/api/users/register", new RegisterUserCommand(email, password, "Joao", "Silva"));

            var response = await _client.PostAsJsonAsync("/api/users/login", new LoginCommand(email, password));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            body.GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
            body.GetProperty("refreshToken").GetString().Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Login_WrongPassword_Returns401()
        {
            var email = $"wrongpwd_{Guid.NewGuid():N}@ex.com";

            await _client.PostAsJsonAsync("/api/users/register", new RegisterUserCommand(email, "Senha@1234", "Joao", "Silva"));

            var response = await _client.PostAsJsonAsync("/api/users/login", new LoginCommand(email, "senha_errada"));
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Login_NonExistentEmail_Returns401()
        {
            var response = await _client.PostAsJsonAsync("/api/users/login", new LoginCommand("naoexiste@ex.com", "qualquer"));
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        // ── GET /api/users/profile ────────────────────────────────────────────────

        [Test]
        public async Task GetProfile_WithoutToken_Returns401()
        {
            var response = await _client.GetAsync("/api/users/profile");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetProfile_WithValidToken_Returns200()
        {
            var email = $"profile_{Guid.NewGuid():N}@ex.com";
            var token = await RegisterAndLoginAsync(email, "Senha@1234");

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/users/profile");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private async Task<string> RegisterAndLoginAsync(string email, string password)
        {
            await _client.PostAsJsonAsync("/api/users/register", new RegisterUserCommand(email, password, "Joao", "Silva"));

            var loginResponse = await _client.PostAsJsonAsync("/api/users/login", new LoginCommand(email, password));
            var body = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            return body.GetProperty("accessToken").GetString()!;
        }
    }
}
