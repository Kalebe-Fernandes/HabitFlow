namespace HabitFlow.Aplicacao.Common.Interfaces
{
    /// <summary>
    /// Contract for authentication operations. Defined in the Application layer
    /// so that handlers can depend on it without referencing Infrastructure.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>Generates a signed JWT access token for the given user.</summary>
        string GenerateAccessToken(Guid userId, string email);

        /// <summary>Generates a cryptographically random opaque refresh token.</summary>
        string GenerateRefreshToken();

        /// <summary>Hashes a plain-text password using PBKDF2-SHA256 with 100,000 iterations.</summary>
        string HashPassword(string password);

        /// <summary>Verifies a plain-text password against a stored PBKDF2 hash.</summary>
        bool VerifyPassword(string password, string hash);
    }
}
