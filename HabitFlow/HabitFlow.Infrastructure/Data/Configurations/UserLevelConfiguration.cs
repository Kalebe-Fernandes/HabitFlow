using HabitFlow.Domain.Gamification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class UserLevelConfiguration : IEntityTypeConfiguration<UserLevel>
    {
        public void Configure(EntityTypeBuilder<UserLevel> builder)
        {
            builder.ToTable("UserLevels", "gamification");

            builder.HasKey(ul => ul.Id);

            builder.Property(ul => ul.CurrentLevel)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(ul => ul.TotalXP)
                .IsRequired()
                .HasDefaultValue(0L);

            builder.HasIndex(ul => ul.Id);

            builder.Property(ul => ul.CreatedAt).IsRequired();
            builder.Property(ul => ul.UpdatedAt).IsRequired();

            builder.Ignore(ul => ul.DomainEvents);
        }
    }
}
