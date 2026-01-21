using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Infrastructure.Data
{
    /// <summary>
    /// Implementation of Unit of Work pattern using Entity Framework Core.
    /// </summary>
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
