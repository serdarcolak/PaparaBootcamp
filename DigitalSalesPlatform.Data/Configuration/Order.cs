using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalSalesPlatform.Data.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderNumber).IsRequired();
        builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
        builder.Property(o => o.WalletUsed).HasPrecision(18, 2);
        builder.Property(o => o.CouponAmount).HasPrecision(18, 2);
        builder.Property(o => o.PointsUsed).HasPrecision(18, 2);
        builder.Property(o => o.PointsEarned).HasPrecision(18, 2);
        builder.Property(o => o.OrderDate).IsRequired();

        builder.HasOne(o => o.User)
            .WithMany(u=> u.Orders)
            .HasForeignKey(o => o.UserId);
    }
}