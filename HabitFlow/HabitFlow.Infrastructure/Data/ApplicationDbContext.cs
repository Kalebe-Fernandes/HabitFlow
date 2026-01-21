using HabitFlow.Domain.Gamification;
using HabitFlow.Domain.Goals;
using HabitFlow.Domain.Habits;
using HabitFlow.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HabitFlow.Infrastructure.Data
{
    /// <summary>
    /// Application database context with support for multiple schemas (bounded contexts).
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        // Users Schema
        public DbSet<User> Users => Set<User>();

        // Habits Schema
        public DbSet<Habit> Habits => Set<Habit>();
        public DbSet<HabitCompletion> HabitCompletions => Set<HabitCompletion>();

        // Goals Schema
        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<GoalHabit> GoalHabits => Set<GoalHabit>();

        // Gamification Schema
        public DbSet<UserLevel> UserLevels => Set<UserLevel>();
        public DbSet<UserBadge> UserBadges => Set<UserBadge>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configure schemas for bounded contexts
            modelBuilder.HasDefaultSchema("dbo");
        }

        /// <summary>
        /// Override SaveChanges to handle audit fields and domain events.
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Set audit fields
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            // Save changes
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
