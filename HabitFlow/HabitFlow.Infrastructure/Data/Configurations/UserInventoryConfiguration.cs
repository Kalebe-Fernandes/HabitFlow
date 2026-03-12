using HabitFlow.Domain.Gamification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabitFlow.Infrastructure.Data.Configurations
{
    public class UserInventoryConfiguration : IEntityTypeConfiguration<UserInventory>
    {
        public void Configure(EntityTypeBuilder<UserInventory> builder)
        {
            builder.ToTable("UserInventories", "gamification");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.StoreItemId).IsRequired();
            builder.Property(u => u.PurchasedAt).IsRequired();
            builder.Property(u => u.IsEquipped).IsRequired();

            builder.HasOne(u => u.StoreItem)
                .WithMany()
                .HasForeignKey(u => u.StoreItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // A user can own the same item only once
            builder.HasIndex(u => new { u.UserId, u.StoreItemId }).IsUnique();
            builder.HasIndex(u => u.UserId);
        }
    }
}
