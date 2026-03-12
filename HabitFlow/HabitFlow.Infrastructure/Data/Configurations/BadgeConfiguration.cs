using HabitFlow.Domain.Gamification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
    {
        public void Configure(EntityTypeBuilder<Badge> builder)
        {
            builder.ToTable("Badges", "gamification");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.IconUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.Rarity)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(b => b.IsActive).IsRequired();

            // BadgeCriteria owned value object
            builder.OwnsOne(b => b.Criteria, criteria =>
            {
                criteria.Property(c => c.Type)
                    .HasColumnName("CriteriaType")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                criteria.Property(c => c.TargetValue)
                    .HasColumnName("CriteriaTargetValue")
                    .IsRequired();
            });

            builder.HasIndex(b => b.Rarity);
            builder.HasIndex(b => b.IsActive);

            builder.Property(b => b.CreatedAt).IsRequired();
            builder.Property(b => b.UpdatedAt).IsRequired();
        }
    }
}
