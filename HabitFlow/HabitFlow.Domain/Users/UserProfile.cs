using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Users
{
    public sealed class UserProfile : ValueObject
    {
        public string? Bio { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string Timezone { get; private set; } = "America/Sao_Paulo";
        public string Language { get; private set; } = "pt-BR";
        private UserProfile() { }
        public static UserProfile CreateDefault() => new UserProfile();
        public static UserProfile Create(string? bio, DateTime? dateOfBirth, string timezone, string language)
            => new() { Bio = bio, DateOfBirth = dateOfBirth, Timezone = timezone, Language = language };
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Bio;
            yield return DateOfBirth;
            yield return Timezone;
            yield return Language;
        }
    }
}
