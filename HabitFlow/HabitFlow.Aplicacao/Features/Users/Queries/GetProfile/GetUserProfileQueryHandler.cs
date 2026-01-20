using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Users.Queries.GetProfile
{
    public class GetUserProfileQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserProfileQuery, Result<UserProfileResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null) return Result.Failure<UserProfileResponse>("User not found");

            var response = new UserProfileResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Profile?.AvatarUrl,
                user.Profile?.Bio,
                user.CreatedAt
            );

            return Result.Success(response);
        }
    }
}
