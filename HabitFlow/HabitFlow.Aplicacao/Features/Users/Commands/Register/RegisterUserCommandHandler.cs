using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using HabitFlow.Domain.Users;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Users.Commands.Register
{
    public class RegisterUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService, IUnitOfWork unitOfWork) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingUser is not null)
                return Result.Failure<RegisterUserResponse>("Email already registered");

            var passwordHash = _authenticationService.HashPassword(request.Password);
            var user = User.Create(request.Email, passwordHash, request.FirstName, request.LastName);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new RegisterUserResponse(user.Id, user.Email));
        }
    }
}
