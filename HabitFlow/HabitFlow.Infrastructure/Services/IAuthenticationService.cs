namespace HabitFlow.Infrastructure.Services
{
    public interface IAuthenticationService
    {
        string GenerateAccessToken(Guid userId, string email);
        string GenerateRefreshToken();
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
