using FluentAssertions;
using HabitFlow.Aplicacao.Features.Habits.Commands.CompleteHabit;
using HabitFlow.Aplicacao.Features.Habits.Commands.CreateHabit;
using HabitFlow.Aplicacao.Features.Users.Commands.Login;
using HabitFlow.Aplicacao.Features.Users.Commands.Register;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace HabitFlow.IntegrationTests.Controllers
{
    [TestFixture]
    public class HabitsControllerTests
    {
        private HabitFlowWebApplicationFactory _factory = null!;
        private HttpClient _client = null!;

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

        private static CreateHabitRequest DailyBinaryRequest() =>
            new("Meditar", "Meditacao diaria", "meditation", "#4CAF50",
                "Daily", "Binary", null, null, null, null, null, null);

        // ── Authorization guard ───────────────────────────────────────────────────

        [Test]
        public async Task GetHabits_WithoutToken_Returns401()
        {
            var response = await _client.GetAsync("/api/habits");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task CreateHabit_WithoutToken_Returns401()
        {
            var response = await _client.PostAsJsonAsync("/api/habits", DailyBinaryRequest());

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        // ── POST /api/habits ──────────────────────────────────────────────────────

        [Test]
        public async Task CreateHabit_ValidRequest_Returns201()
        {
            await AuthenticateAsync();

            var response = await _client.PostAsJsonAsync("/api/habits", DailyBinaryRequest());

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async Task CreateHabit_ValidRequest_ReturnsHabitIdAndName()
        {
            await AuthenticateAsync();

            var response = await _client.PostAsJsonAsync("/api/habits", DailyBinaryRequest());
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();

            body.GetProperty("habitId").GetGuid().Should().NotBeEmpty();
            body.GetProperty("name").GetString().Should().Be("Meditar");
        }

        [Test]
        public async Task CreateHabit_WeeklyNumeric_Returns201()
        {
            await AuthenticateAsync();

            var request = new CreateHabitRequest(
                "Correr", null, "run", "#FF5722",
                "Weekly", "Numeric", 30m, "minutos",
                42, null, null, null);

            var response = await _client.PostAsJsonAsync("/api/habits", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async Task CreateHabit_MissingName_ReturnsError()
        {
            await AuthenticateAsync();

            var request = new CreateHabitRequest(
                "", null, "meditation", "#4CAF50",
                "Daily", "Binary", null, null, null, null, null, null);

            var response = await _client.PostAsJsonAsync("/api/habits", request);

            response.IsSuccessStatusCode.Should().BeFalse();
        }

        // ── GET /api/habits ───────────────────────────────────────────────────────

        [Test]
        public async Task GetHabits_AuthenticatedWithNoHabits_ReturnsEmptyList()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync("/api/habits");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            body.GetProperty("habits").GetArrayLength().Should().Be(0);
        }

        [Test]
        public async Task GetHabits_AfterCreatingHabit_ReturnsHabit()
        {
            await AuthenticateAsync();
            await _client.PostAsJsonAsync("/api/habits", DailyBinaryRequest());

            var response = await _client.GetAsync("/api/habits");
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();

            body.GetProperty("habits").GetArrayLength().Should().Be(1);
        }

        // ── POST /api/habits/{id}/complete ────────────────────────────────────────

        [Test]
        public async Task CompleteHabit_ValidRequest_Returns200WithStreak()
        {
            await AuthenticateAsync();
            var habitId = await CreateHabitAsync();

            var request = new CompleteHabitRequest(DateTime.UtcNow, null, null, null, null);
            var response = await _client.PostAsJsonAsync(
                $"/api/habits/{habitId}/complete", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            body.GetProperty("currentStreak").GetInt32().Should().Be(1);
        }

        [Test]
        public async Task CompleteHabit_SameDayTwice_ReturnsBadRequest()
        {
            await AuthenticateAsync();
            var habitId = await CreateHabitAsync();
            var request = new CompleteHabitRequest(DateTime.UtcNow, null, null, null, null);

            await _client.PostAsJsonAsync($"/api/habits/{habitId}/complete", request);
            var response = await _client.PostAsJsonAsync($"/api/habits/{habitId}/complete", request);

            // Domain exception → ExceptionHandlingMiddleware → 400
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CompleteHabit_UnknownHabitId_ReturnsBadRequest()
        {
            await AuthenticateAsync();

            var request = new CompleteHabitRequest(DateTime.UtcNow, null, null, null, null);
            var response = await _client.PostAsJsonAsync(
                $"/api/habits/{Guid.NewGuid()}/complete", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // ── PUT /api/habits/{id} ──────────────────────────────────────────────────

        [Test]
        public async Task UpdateHabit_ValidRequest_Returns204()
        {
            await AuthenticateAsync();
            var habitId = await CreateHabitAsync();

            var request = new { Name = "Meditar Profundamente", Description = "Nova desc", IconName = "mind", ColorHex = "#9C27B0" };
            var response = await _client.PutAsJsonAsync($"/api/habits/{habitId}", request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public async Task UpdateHabit_UnknownId_ReturnsBadRequest()
        {
            await AuthenticateAsync();

            var request = new { Name = "Qualquer", Description = (string?)null, IconName = "icon", ColorHex = "#000000" };
            var response = await _client.PutAsJsonAsync($"/api/habits/{Guid.NewGuid()}", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // ── GET /api/habits/{id}/statistics ──────────────────────────────────────

        [Test]
        public async Task GetStatistics_ExistingHabit_Returns200()
        {
            await AuthenticateAsync();
            var habitId = await CreateHabitAsync();

            var response = await _client.GetAsync($"/api/habits/{habitId}/statistics");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetStatistics_UnknownHabit_Returns404()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync($"/api/habits/{Guid.NewGuid()}/statistics");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private async Task AuthenticateAsync()
        {
            var email = $"habits_{Guid.NewGuid():N}@ex.com";
            const string password = "Senha@1234";

            await _client.PostAsJsonAsync("/api/users/register",
                new RegisterUserCommand(email, password, "Joao", "Silva"));

            var loginResponse = await _client.PostAsJsonAsync("/api/users/login",
                new LoginCommand(email, password));

            var body = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = body.GetProperty("accessToken").GetString()!;

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<Guid> CreateHabitAsync()
        {
            var response = await _client.PostAsJsonAsync("/api/habits", DailyBinaryRequest());
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            return body.GetProperty("habitId").GetGuid();
        }
    }
}
