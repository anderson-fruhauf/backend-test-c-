using Store.Application.Orders.Commands.RemoveProductFromOrder;
using Store.Application.Tests.Fakes;
using Store.Domain.Entities;

namespace Store.Application.Tests;

public class RemoveProductFromOrderHandlerTests
{
    private static readonly Product Mouse =
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Mouse", 100m);

    private readonly FakeOrderRepository _orders = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly RemoveProductFromOrderHandler _handler;

    public RemoveProductFromOrderHandlerTests()
    {
        _handler = new RemoveProductFromOrderHandler(_orders, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldRemoveItem()
    {
        var order = await OpenOrderWithMouse();

        var result = await _handler.Handle(new RemoveProductFromOrderCommand(order.Id, Mouse.Id));

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Empty(order.Items);
        Assert.Equal(1, _unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnNullWhenOrderIsMissing()
    {
        var result = await _handler.Handle(new RemoveProductFromOrderCommand(Guid.NewGuid(), Mouse.Id));

        Assert.Null(result);
        Assert.Equal(0, _unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task Handle_ShouldFailWhenProductIsMissing()
    {
        var order = await OpenOrderWithMouse();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new RemoveProductFromOrderCommand(order.Id, Guid.NewGuid())));
    }

    private async Task<Order> OpenOrderWithMouse()
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem(Mouse, 1);
        await _orders.AddAsync(order);
        return order;
    }
}
