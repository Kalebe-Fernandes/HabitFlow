using HabitFlow.Domain.Gamification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class UserBadgeConfiguration : IEntityTypeConfiguration<UserBadge>
    {
        public void Configure(EntityTypeBuilder<UserBadge> builder)
        {
            builder.ToTable("UserBadges", "gamification");

            builder.HasKey(ub => ub.Id);

            builder.Property(ub => ub.BadgeId)
                .IsRequired();

            builder.Property(ub => ub.EarnedAt).IsRequired();
        }
    }
}
