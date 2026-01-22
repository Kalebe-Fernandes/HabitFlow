using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.Services.Email
{
    public class MockEmailService(ILogger<MockEmailService> logger) : IEmailService
    {
        private readonly ILogger<MockEmailService> _logger = logger;

        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[MOCK EMAIL] To: {To} | Subject: {Subject}", to, subject);
            return Task.CompletedTask;
        }

        public Task SendPasswordResetEmailAsync(string to, string resetToken, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[MOCK EMAIL] Password Reset to {To} | Token: {Token}", to, resetToken);
            return Task.CompletedTask;
        }

        public Task SendEmailConfirmationAsync(string to, string confirmToken, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[MOCK EMAIL] Email Confirmation to {To} | Token: {Token}", to, confirmToken);
            return Task.CompletedTask;
        }
    }
}
