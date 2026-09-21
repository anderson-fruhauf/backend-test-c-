using Store.Application.Orders.Commands.CloseOrder;
using Store.Application.Tests.Fakes;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Application.Tests;

public class CloseOrderHandlerTests
{
    private readonly FakeOrderRepository _orders = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly CloseOrderHandler _handler;

    public CloseOrderHandlerTests()
    {
        _handler = new CloseOrderHandler(_orders, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldCloseOrder()
    {
        var order = await OpenOrder();

        var result = await _handler.Handle(new CloseOrderCommand(order.Id));

        Assert.NotNull(result);
        Assert.Equal(nameof(OrderStatus.Closed), result.Status);
        Assert.NotNull(result.ClosedAt);
        Assert.Equal(OrderStatus.Closed, order.Status);
        Assert.Equal(1, _unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnNullWhenOrderIsMissing()
    {
        var result = await _handler.Handle(new CloseOrderCommand(Guid.NewGuid()));

        Assert.Null(result);
        Assert.Equal(0, _unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task Handle_ShouldFailWhenAlreadyClosed()
    {
        var order = await OpenOrder();
        order.Close();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new CloseOrderCommand(order.Id)));
    }

    private async Task<Order> OpenOrder()
    {
        var order = new Order(Guid.NewGuid());
        await _orders.AddAsync(order);
        return order;
    }
}
