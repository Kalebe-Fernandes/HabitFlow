using HabitFlow.Domain.Gamification;
using HabitFlow.Domain.Goals;
using HabitFlow.Domain.Habits;
using HabitFlow.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace HabitFlow.Infrastructure.Data
{
    /// <summary>
    /// Application database context. Covers all bounded contexts via separate schemas.
    /// Audit field stamping is handled here to keep it out of the domain.
    /// Domain event dispatch is handled by UnitOfWork after SaveChanges.
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        // Users schema
        public DbSet<User> Users => Set<User>();

        // Habits schema
        public DbSet<Habit> Habits => Set<Habit>();
        public DbSet<HabitCompletion> HabitCompletions => Set<HabitCompletion>();

        // Goals schema
        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<GoalHabit> GoalHabits => Set<GoalHabit>();

        // Gamification schema
        public DbSet<UserLevel> UserLevels => Set<UserLevel>();
        public DbSet<UserBadge> UserBadges => Set<UserBadge>();
        public DbSet<Badge> Badges => Set<Badge>();
        public DbSet<XPTransaction> XPTransactions => Set<XPTransaction>();
        public DbSet<CurrencyTransaction> CurrencyTransactions => Set<CurrencyTransaction>();
        public DbSet<StoreItem> StoreItems => Set<StoreItem>();
        public DbSet<UserInventory> UserInventories => Set<UserInventory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bounded context schemas are applied via individual IEntityTypeConfiguration classes.
            // HasDefaultSchema is intentionally omitted: each configuration declares its own schema,
            // so no fallback default is needed and "dbo" would create misleading expectations.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
