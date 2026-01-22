using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Analytics
{
    /// <summary>
    /// Entity representing aggregated daily metrics for a user.
    /// Used for analytics, reporting, and trend analysis.
    /// </summary>
    public sealed class DailyMetric : Entity<long>
    {
        /// <summary>
        /// Gets the user identifier for these metrics.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the date for these metrics.
        /// </summary>
        public DateOnly Date { get; private set; }

        /// <summary>
        /// Gets the number of habits completed on this day.
        /// </summary>
        public int CompletedHabits { get; private set; }

        /// <summary>
        /// Gets the total number of active habits on this day.
        /// </summary>
        public int TotalHabits { get; private set; }

        /// <summary>
        /// Gets the completion rate (0-100).
        /// </summary>
        public decimal CompletionRate { get; private set; }

        /// <summary>
        /// Gets the XP earned on this day.
        /// </summary>
        public int XPEarned { get; private set; }

        /// <summary>
        /// Gets the virtual currency earned on this day.
        /// </summary>
        public int CoinsEarned { get; private set; }

        /// <summary>
        /// Gets the average mood level (1-5) if recorded.
        /// </summary>
        public decimal? AverageMood { get; private set; }

        /// <summary>
        /// Gets the average energy level (1-5) if recorded.
        /// </summary>
        public decimal? AverageEnergy { get; private set; }

        /// <summary>
        /// Gets the number of active streaks on this day.
        /// </summary>
        public int ActiveStreaks { get; private set; }

        /// <summary>
        /// Gets the longest streak on this day.
        /// </summary>
        public int LongestStreak { get; private set; }

        private DailyMetric() { }

        /// <summary>
        /// Creates daily metrics for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date for these metrics.</param>
        /// <param name="completedHabits">Number of habits completed.</param>
        /// <param name="totalHabits">Total number of active habits.</param>
        /// <param name="xpEarned">XP earned on this day.</param>
        /// <param name="coinsEarned">Coins earned on this day.</param>
        /// <param name="averageMood">Optional average mood level.</param>
        /// <param name="averageEnergy">Optional average energy level.</param>
        /// <param name="activeStreaks">Number of active streaks.</param>
        /// <param name="longestStreak">The longest streak value.</param>
        /// <returns>A new DailyMetric instance.</returns>
        public static DailyMetric Create(
            Guid userId,
            DateOnly date,
            int completedHabits,
            int totalHabits,
            int xpEarned,
            int coinsEarned,
            decimal? averageMood = null,
            decimal? averageEnergy = null,
            int activeStreaks = 0,
            int longestStreak = 0)
        {
            var completionRate = totalHabits > 0
                ? Math.Round((decimal)completedHabits / totalHabits * 100, 2)
                : 0m;

            return new DailyMetric
            {
                UserId = userId,
                Date = date,
                CompletedHabits = completedHabits,
                TotalHabits = totalHabits,
                CompletionRate = completionRate,
                XPEarned = xpEarned,
                CoinsEarned = coinsEarned,
                AverageMood = averageMood,
                AverageEnergy = averageEnergy,
                ActiveStreaks = activeStreaks,
                LongestStreak = longestStreak,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Updates the metrics for this day.
        /// </summary>
        /// <param name="completedHabits">Number of habits completed.</param>
        /// <param name="totalHabits">Total number of active habits.</param>
        /// <param name="xpEarned">XP earned on this day.</param>
        /// <param name="coinsEarned">Coins earned on this day.</param>
        /// <param name="averageMood">Optional average mood level.</param>
        /// <param name="averageEnergy">Optional average energy level.</param>
        /// <param name="activeStreaks">Number of active streaks.</param>
        /// <param name="longestStreak">The longest streak value.</param>
        public void Update(
            int completedHabits,
            int totalHabits,
            int xpEarned,
            int coinsEarned,
            decimal? averageMood = null,
            decimal? averageEnergy = null,
            int activeStreaks = 0,
            int longestStreak = 0)
        {
            CompletedHabits = completedHabits;
            TotalHabits = totalHabits;
            CompletionRate = totalHabits > 0
                ? Math.Round((decimal)completedHabits / totalHabits * 100, 2)
                : 0m;
            XPEarned = xpEarned;
            CoinsEarned = coinsEarned;
            AverageMood = averageMood;
            AverageEnergy = averageEnergy;
            ActiveStreaks = activeStreaks;
            LongestStreak = longestStreak;
        }
    }
}
