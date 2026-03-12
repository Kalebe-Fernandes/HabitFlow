using HabitFlow.Domain.Gamification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class StoreItemConfiguration : IEntityTypeConfiguration<StoreItem>
    {
        public void Configure(EntityTypeBuilder<StoreItem> builder)
        {
            builder.ToTable("StoreItems", "gamification");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(s => s.Price).IsRequired();

            builder.Property(s => s.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.ItemData)
                .HasMaxLength(2000);

            builder.Property(s => s.IsActive).IsRequired();
            builder.Property(s => s.IsFeatured).IsRequired();
            builder.Property(s => s.SortOrder).IsRequired();

            builder.HasIndex(s => s.Category);
            builder.HasIndex(s => s.IsActive);
            builder.HasIndex(s => s.SortOrder);

            builder.Property(s => s.CreatedAt).IsRequired();
            builder.Property(s => s.UpdatedAt).IsRequired();
        }
    }
}
