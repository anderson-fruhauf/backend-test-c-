using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Domain.Tests;

public class OrderTests
{
    private static Product Mouse(decimal price = 100m) =>
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Mouse", price);

    [Fact]
    public void Constructor_ShouldStartOpen()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Equal(OrderStatus.Open, order.Status);
        Assert.Null(order.ClosedAt);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyId()
    {
        Assert.Throws<ArgumentException>(() => new Order(Guid.Empty));
    }

    [Fact]
    public void Close_ShouldSetClosedStatusAndDate()
    {
        var order = new Order(Guid.NewGuid());

        order.Close();

        Assert.Equal(OrderStatus.Closed, order.Status);
        Assert.NotNull(order.ClosedAt);
    }

    [Fact]
    public void Close_ShouldFailWhenAlreadyClosed()
    {
        var order = new Order(Guid.NewGuid());
        order.Close();

        Assert.Throws<InvalidOperationException>(() => order.Close());
    }

    [Fact]
    public void AddItem_ShouldAddProduct()
    {
        var order = new Order(Guid.NewGuid());

        order.AddItem(Mouse(), 2);

        var item = Assert.Single(order.Items);
        Assert.Equal("Mouse", item.ProductName);
        Assert.Equal(100m, item.UnitPrice);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(200m, item.Total);
    }

    [Fact]
    public void AddItem_ShouldIncreaseQuantityWhenProductAlreadyExists()
    {
        var order = new Order(Guid.NewGuid());
        var mouse = Mouse();

        order.AddItem(mouse, 1);
        order.AddItem(mouse, 2);

        var item = Assert.Single(order.Items);
        Assert.Equal(3, item.Quantity);
    }

    [Fact]
    public void AddItem_ShouldKeepOriginalPriceWhenProductIsAddedAgain()
    {
        var order = new Order(Guid.NewGuid());
        var id = Guid.NewGuid();

        order.AddItem(new Product(id, "Mouse", 100m), 1);
        order.AddItem(new Product(id, "Mouse", 150m), 1);

        var item = Assert.Single(order.Items);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(100m, item.UnitPrice);
    }

    [Fact]
    public void AddItem_ShouldRejectNonPositiveQuantity()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Throws<ArgumentOutOfRangeException>(() => order.AddItem(Mouse(), 0));
    }

    [Fact]
    public void AddItem_ShouldFailWhenOrderIsClosed()
    {
        var order = new Order(Guid.NewGuid());
        order.Close();

        Assert.Throws<InvalidOperationException>(() => order.AddItem(Mouse(), 1));
    }

    [Fact]
    public void RemoveItem_ShouldRemoveProduct()
    {
        var order = new Order(Guid.NewGuid());
        var mouse = Mouse();
        order.AddItem(mouse, 1);

        order.RemoveItem(mouse.Id);

        Assert.Empty(order.Items);
    }

    [Fact]
    public void RemoveItem_ShouldFailWhenProductIsMissing()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Throws<InvalidOperationException>(() => order.RemoveItem(Guid.NewGuid()));
    }

    [Fact]
    public void RemoveItem_ShouldFailWhenOrderIsClosed()
    {
        var order = new Order(Guid.NewGuid());
        var mouse = Mouse();
        order.AddItem(mouse, 1);
        order.Close();

        Assert.Throws<InvalidOperationException>(() => order.RemoveItem(mouse.Id));
    }
}
