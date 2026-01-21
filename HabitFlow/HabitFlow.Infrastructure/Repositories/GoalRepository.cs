using HabitFlow.Domain.Goals;
using HabitFlow.Domain.Repositories;
using HabitFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitFlow.Infrastructure.Repositories
{
    public class GoalRepository(ApplicationDbContext context) : IGoalRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Goal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Goals
                .Include(g => g.GoalHabits)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<List<Goal>> GetUserGoalsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Goals
                .Include(g => g.GoalHabits)
                .Where(g => g.UserId == userId && g.Status == GoalStatus.Active)
                .OrderBy(g => g.TargetDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Goal aggregate, CancellationToken cancellationToken = default)
        {
            await _context.Goals.AddAsync(aggregate, cancellationToken);
        }

        public void Update(Goal aggregate)
        {
            _context.Goals.Update(aggregate);
        }

        public void Remove(Goal aggregate)
        {
            _context.Goals.Remove(aggregate);
        }

        public async Task<IEnumerable<Goal>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await GetUserGoalsAsync(userId, cancellationToken);
        }
    }
}
