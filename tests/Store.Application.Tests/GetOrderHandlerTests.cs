using Store.Application.Orders.Queries.GetOrder;
using Store.Application.Tests.Fakes;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Application.Tests;

public class GetOrderHandlerTests
{
    private readonly FakeOrderRepository _orders = new();
    private readonly GetOrderHandler _handler;

    public GetOrderHandlerTests()
    {
        _handler = new GetOrderHandler(_orders);
    }

    [Fact]
    public async Task Handle_ShouldReturnOrder()
    {
        var order = new Order(Guid.NewGuid());
        await _orders.AddAsync(order);

        var result = await _handler.Handle(new GetOrderQuery(order.Id));

        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
        Assert.Equal(nameof(OrderStatus.Open), result.Status);
    }

    [Fact]
    public async Task Handle_ShouldReturnNullWhenOrderIsMissing()
    {
        var result = await _handler.Handle(new GetOrderQuery(Guid.NewGuid()));

        Assert.Null(result);
    }
}
