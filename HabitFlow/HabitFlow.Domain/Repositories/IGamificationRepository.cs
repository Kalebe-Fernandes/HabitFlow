using HabitFlow.Domain.Common.Interfaces;
using HabitFlow.Domain.Gamification;

namespace HabitFlow.Domain.Repositories
{
    public interface IGamificationRepository : IRepository<UserLevel, Guid>
    {
        Task<UserLevel?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
