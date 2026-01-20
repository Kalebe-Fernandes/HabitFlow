using HabitFlow.Domain.Common.Interfaces;
using HabitFlow.Domain.Goals;

namespace HabitFlow.Domain.Repositories
{
    public interface IGoalRepository : IRepository<Goal, Guid>
    {
        Task<IEnumerable<Goal>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
