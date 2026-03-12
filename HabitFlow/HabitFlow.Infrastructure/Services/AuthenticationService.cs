using HabitFlow.Aplicacao.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HabitFlow.Infrastructure.Services
{
    /// <summary>
    /// Implements IAuthenticationService using PBKDF2-SHA256 for password hashing
    /// and HMAC-SHA256 signed JWTs for access tokens.
    /// </summary>
    public class AuthenticationService(IConfiguration configuration) : IAuthenticationService
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateAccessToken(Guid userId, string email)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "15")),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string HashPassword(string password)
        {
            // Gera um salt aleatório de 16 bytes
            var salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            // Deriva a chave (hash) usando o método estático Pbkdf2
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

            // Armazena salt (16 bytes) + hash (32 bytes) em um único Base64
            var hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashBytes = Convert.FromBase64String(hash);

            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Usa o método estático Pbkdf2 em vez do construtor obsoleto
            var testHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

            // Constant-time comparison to prevent timing attacks
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != testHash[i])
                    return false;
            }

            return true;
        }
    }
}
