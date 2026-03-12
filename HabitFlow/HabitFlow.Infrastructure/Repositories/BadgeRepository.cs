using HabitFlow.Domain.Gamification;
using HabitFlow.Domain.Gamification.Enums;
using HabitFlow.Domain.Repositories;
using HabitFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitFlow.Infrastructure.Repositories
{
    public class BadgeRepository(ApplicationDbContext context) : IBadgeRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Badge?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Badges.FindAsync([id], cancellationToken);
        }

        public async Task<IReadOnlyList<Badge>> GetActiveBadgesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Badges
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Badge>> GetByRarityAsync(BadgeRarity rarity, CancellationToken cancellationToken = default)
        {
            return await _context.Badges
                .Where(b => b.Rarity == rarity && b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Badge>> GetByCriteriaTypeAsync(BadgeCriteriaType criteriaType, CancellationToken cancellationToken = default)
        {
            return await _context.Badges
                .Where(b => b.Criteria.Type == criteriaType && b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Badge aggregate, CancellationToken cancellationToken = default)
        {
            await _context.Badges.AddAsync(aggregate, cancellationToken);
        }

        public void Update(Badge aggregate)
        {
            _context.Badges.Update(aggregate);
        }

        public void Remove(Badge aggregate)
        {
            _context.Badges.Remove(aggregate);
        }
    }
}
