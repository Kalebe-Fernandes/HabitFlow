using HabitFlow.Domain.Repositories;
using HabitFlow.Domain.Users;
using HabitFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitFlow.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync([id], cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
        }

        public async Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
        }

        public async Task AddAsync(User aggregate, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(aggregate, cancellationToken);
        }

        public void Update(User aggregate)
        {
            _context.Users.Update(aggregate);
        }

        public void Remove(User aggregate)
        {
            _context.Users.Remove(aggregate);
        }
    }
}
