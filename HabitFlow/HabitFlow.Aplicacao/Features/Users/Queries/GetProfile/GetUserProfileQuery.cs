using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Users.Queries.GetProfile
{
    public record GetUserProfileQuery(Guid UserId) : IQuery<UserProfileResponse>;

    public record UserProfileResponse(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string? AvatarUrl,
        string? Bio,
        DateTime CreatedAt
    );

}
