using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Notifications.Enums;

namespace HabitFlow.Domain.Notifications
{
    /// <summary>
    /// Entity representing a notification/reminder for a user.
    /// Supports habit reminders, achievement alerts, and social notifications.
    /// </summary>
    public sealed class Notification : Entity<long>
    {
        private const int MaxTitleLength = 200;
        private const int MaxBodyLength = 1000;

        /// <summary>
        /// Gets the user identifier who will receive this notification.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the type of notification.
        /// </summary>
        public NotificationType Type { get; private set; }

        /// <summary>
        /// Gets the notification title.
        /// </summary>
        public string Title { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the notification body/message.
        /// </summary>
        public string Body { get; private set; } = string.Empty;

        /// <summary>
        /// Gets additional data in JSON format (habit ID, badge ID, etc.).
        /// </summary>
        public string? Data { get; private set; }

        /// <summary>
        /// Gets whether the notification has been read by the user.
        /// </summary>
        public bool IsRead { get; private set; }

        /// <summary>
        /// Gets whether the notification has been sent.
        /// </summary>
        public bool IsSent { get; private set; }

        /// <summary>
        /// Gets when the notification should be sent.
        /// </summary>
        public DateTime ScheduledFor { get; private set; }

        /// <summary>
        /// Gets when the notification was actually sent (null if not sent yet).
        /// </summary>
        public DateTime? SentAt { get; private set; }

        /// <summary>
        /// Gets when the notification was read (null if not read yet).
        /// </summary>
        public DateTime? ReadAt { get; private set; }

        private Notification() { }

        /// <summary>
        /// Creates a new notification for a habit reminder.
        /// </summary>
        /// <param name="userId">The user to notify.</param>
        /// <param name="habitId">The habit to remind about.</param>
        /// <param name="habitName">The name of the habit.</param>
        /// <param name="scheduledFor">When to send the notification.</param>
        /// <returns>A new Notification instance.</returns>
        public static Notification CreateHabitReminder(Guid userId, Guid habitId, string habitName, DateTime scheduledFor)
        {
            return new Notification
            {
                UserId = userId,
                Type = NotificationType.HabitReminder,
                Title = "Lembrete de Hábito",
                Body = $"Hora de completar: {habitName}",
                Data = $"{{\"habitId\":\"{habitId}\"}}",
                IsRead = false,
                IsSent = false,
                ScheduledFor = scheduledFor,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a new notification for a badge earned achievement.
        /// </summary>
        /// <param name="userId">The user to notify.</param>
        /// <param name="badgeId">The badge that was earned.</param>
        /// <param name="badgeName">The name of the badge.</param>
        /// <returns>A new Notification instance.</returns>
        public static Notification CreateBadgeEarned(Guid userId, int badgeId, string badgeName)
        {
            return new Notification
            {
                UserId = userId,
                Type = NotificationType.Achievement,
                Title = "Nova Conquista!",
                Body = $"Você conquistou: {badgeName}",
                Data = $"{{\"badgeId\":{badgeId}}}",
                IsRead = false,
                IsSent = false,
                ScheduledFor = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a new notification for a level up event.
        /// </summary>
        /// <param name="userId">The user to notify.</param>
        /// <param name="newLevel">The new level reached.</param>
        /// <returns>A new Notification instance.</returns>
        public static Notification CreateLevelUp(Guid userId, int newLevel)
        {
            return new Notification
            {
                UserId = userId,
                Type = NotificationType.Achievement,
                Title = "Parabéns!",
                Body = $"Você alcançou o nível {newLevel}!",
                Data = $"{{\"level\":{newLevel}}}",
                IsRead = false,
                IsSent = false,
                ScheduledFor = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a new notification for a streak milestone.
        /// </summary>
        /// <param name="userId">The user to notify.</param>
        /// <param name="habitId">The habit with the streak.</param>
        /// <param name="habitName">The name of the habit.</param>
        /// <param name="streakDays">The streak milestone reached.</param>
        /// <returns>A new Notification instance.</returns>
        public static Notification CreateStreakMilestone(Guid userId, Guid habitId, string habitName, int streakDays)
        {
            return new Notification
            {
                UserId = userId,
                Type = NotificationType.Achievement,
                Title = "Sequência Incrível!",
                Body = $"{streakDays} dias de sequência em {habitName}!",
                Data = $"{{\"habitId\":\"{habitId}\",\"streak\":{streakDays}}}",
                IsRead = false,
                IsSent = false,
                ScheduledFor = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a custom notification.
        /// </summary>
        /// <param name="userId">The user to notify.</param>
        /// <param name="type">The notification type.</param>
        /// <param name="title">The notification title.</param>
        /// <param name="body">The notification body.</param>
        /// <param name="data">Optional additional data.</param>
        /// <param name="scheduledFor">When to send the notification.</param>
        /// <returns>A new Notification instance.</returns>
        public static Notification Create(Guid userId, NotificationType type, string title, string body, string? data = null, DateTime? scheduledFor = null)
        {
            ValidateTitle(title);
            ValidateBody(body);

            return new Notification
            {
                UserId = userId,
                Type = type,
                Title = title.Trim(),
                Body = body.Trim(),
                Data = data?.Trim(),
                IsRead = false,
                IsSent = false,
                ScheduledFor = scheduledFor ?? DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Marks the notification as sent.
        /// </summary>
        public void MarkAsSent()
        {
            if (IsSent) return;
            IsSent = true;
            SentAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the notification as read by the user.
        /// </summary>
        public void MarkAsRead()
        {
            if (IsRead) return;
            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Reschedules the notification for a different time.
        /// </summary>
        /// <param name="newScheduledTime">The new scheduled time.</param>
        public void Reschedule(DateTime newScheduledTime)
        {
            if (IsSent)
                throw new InvalidOperationException("Cannot reschedule a notification that has already been sent");

            ScheduledFor = newScheduledTime;
        }

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            if (title.Length > MaxTitleLength)
                throw new ArgumentException($"Title cannot exceed {MaxTitleLength} characters", nameof(title));
        }

        private static void ValidateBody(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("Body cannot be empty", nameof(body));

            if (body.Length > MaxBodyLength)
                throw new ArgumentException($"Body cannot exceed {MaxBodyLength} characters", nameof(body));
        }
    }
}
