using HabitFlow.Domain.Gamification;
using HabitFlow.Domain.Repositories;
using HabitFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitFlow.Infrastructure.Repositories
{
    public class UserLevelRepository(ApplicationDbContext context) : IUserLevelRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<UserLevel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.UserLevels
                .Include(ul => ul.Badges)
                .FirstOrDefaultAsync(ul => ul.Id == id, cancellationToken);
        }

        public async Task AddAsync(UserLevel aggregate, CancellationToken cancellationToken = default)
        {
            await _context.UserLevels.AddAsync(aggregate, cancellationToken);
        }

        public void Update(UserLevel aggregate)
        {
            _context.UserLevels.Update(aggregate);
        }

        public void Remove(UserLevel aggregate)
        {
            _context.UserLevels.Remove(aggregate);
        }

        public async Task<UserLevel?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserLevels.Include(ul => ul.Badges).FirstOrDefaultAsync(ul => ul.Id == userId, cancellationToken: cancellationToken);
        }
    }
}
