using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Domain.Common.Interfaces;
using MediatR;

namespace HabitFlow.Infrastructure.Data
{
    /// <summary>
    /// Unit of Work implementation using EF Core.
    /// Collects domain events from all tracked aggregates before saving,
    /// persists changes, then dispatches the collected events via MediatR.
    /// Events are published after a successful save to guarantee consistency:
    /// handlers observe state that is already committed.
    /// </summary>
    public class UnitOfWork(ApplicationDbContext context, IMediator mediator) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMediator _mediator = mediator;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Collect all domain events from tracked aggregates before saving
            var aggregatesWithEvents = _context.ChangeTracker
                .Entries<IHasDomainEvents>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = aggregatesWithEvents
                .SelectMany(a => a.DomainEvents)
                .ToList();

            // Persist to database
            var result = await _context.SaveChangesAsync(cancellationToken);

            // Clear events only after a successful save
            foreach (var aggregate in aggregatesWithEvents)
                aggregate.ClearDomainEvents();

            // Dispatch events in order; handlers observe committed state
            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);

            return result;
        }
    }
}
