namespace HabitFlow.Infrastructure.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
        Task SendPasswordResetEmailAsync(string to, string resetToken, CancellationToken cancellationToken = default);
        Task SendEmailConfirmationAsync(string to, string confirmToken, CancellationToken cancellationToken = default);
    }
}
