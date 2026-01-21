using HabitFlow.Domain.Common.Interfaces;
using HabitFlow.Domain.Users;

namespace HabitFlow.Domain.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
