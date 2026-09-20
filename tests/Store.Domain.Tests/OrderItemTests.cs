using Store.Domain.Entities;

namespace Store.Domain.Tests;

public class OrderItemTests
{
    [Fact]
    public void Total_ShouldBeUnitPriceTimesQuantity()
    {
        var item = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Mouse", 3, 100m);

        Assert.Equal(300m, item.Total);
    }

    [Fact]
    public void IncreaseQuantity_ShouldAddToQuantity()
    {
        var item = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Mouse", 1, 100m);

        item.IncreaseQuantity(2);

        Assert.Equal(3, item.Quantity);
        Assert.Equal(300m, item.Total);
    }

    [Fact]
    public void IncreaseQuantity_ShouldRejectNonPositiveQuantity()
    {
        var item = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Mouse", 1, 100m);

        Assert.Throws<ArgumentOutOfRangeException>(() => item.IncreaseQuantity(0));
    }

    [Fact]
    public void Constructor_ShouldRejectNonPositiveQuantity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Mouse", 0, 100m));
    }
}
