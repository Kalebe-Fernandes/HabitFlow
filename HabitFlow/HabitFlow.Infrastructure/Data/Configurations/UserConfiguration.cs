using HabitFlow.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.DisplayName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.AvatarUrl)
                .HasMaxLength(500);

            // Owned types for UserProfile and UserSettings
            builder.OwnsOne(u => u.Profile, profile =>
            {
                profile.Property(p => p.Bio)
                    .HasMaxLength(500);

                profile.Property(p => p.Timezone)
                    .HasMaxLength(100)
                    .HasDefaultValue("America/Sao_Paulo");

                profile.Property(p => p.Language)
                    .HasMaxLength(10)
                    .HasDefaultValue("pt-BR");
            });

            builder.OwnsOne(u => u.Settings, settings =>
            {
                settings.Property(s => s.NotificationsEnabled)
                    .HasDefaultValue(true);
            });

            // Audit fields
            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .IsRequired();

            // Ignore domain events
            builder.Ignore(u => u.DomainEvents);
        }
    }
}
