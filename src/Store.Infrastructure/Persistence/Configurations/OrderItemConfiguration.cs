namespace Store.Infrastructure.Persistence.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedNever();
        builder.Property(item => item.Quantity).IsRequired();
        builder.Property(item => item.UnitPrice).HasPrecision(18, 2);
        builder.Property(item => item.ProductName).IsRequired();
        builder.Ignore(item => item.Total);

    }
}