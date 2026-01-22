using HabitFlow.Domain.Habits;
using HabitFlow.Domain.Habits.Enums;
using HabitFlow.Domain.Repositories;
using HabitFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitFlow.Infrastructure.Repositories
{
    public class HabitRepository(ApplicationDbContext context) : IHabitRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Habit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Habits.FindAsync([id], cancellationToken);
        }

        public async Task<List<Habit>> GetUserHabitsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Habits
                .Where(h => h.UserId == userId && h.Status == HabitStatus.Active)
                .OrderBy(h => h.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<HabitCompletion>> GetCompletionsAsync(Guid habitId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.HabitCompletions
                .Where(hc => hc.HabitId == habitId &&
                            hc.CompletionDate >= DateOnly.FromDateTime(startDate) &&
                            hc.CompletionDate <= DateOnly.FromDateTime(endDate))
                .OrderBy(hc => hc.CompletionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Habit aggregate, CancellationToken cancellationToken = default)
        {
            await _context.Habits.AddAsync(aggregate, cancellationToken);
        }

        public void Update(Habit aggregate)
        {
            _context.Habits.Update(aggregate);
        }

        public void Remove(Habit aggregate)
        {
            _context.Habits.Remove(aggregate);
        }

        public async Task<IEnumerable<Habit>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var habits = await GetUserHabitsAsync(userId, cancellationToken);
            habits = [.. habits.Where(h => h.Status == HabitStatus.Active)];
            return habits;
        }

        public async Task<(IEnumerable<Habit> Habits, int TotalCount)> GetPagedByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var habits = await GetUserHabitsAsync(userId, cancellationToken);
            return (habits, habits.Count);
        }

        public async Task<Habit?> GetByIdWithCompletionsAsync(Guid habitId, CancellationToken cancellationToken = default)
        {
            return await _context.Habits.Where(h => h.Id == habitId).Include(h => h.Completions).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> ExistsWithNameAsync(Guid userId, string name, Guid? excludeHabitId = null, CancellationToken cancellationToken = default)
        {
            return await _context.Habits.FirstOrDefaultAsync(h => h.Name == name, cancellationToken: cancellationToken) != null;
        }
    }
}
