using Store.Application.Orders.Queries.GetOrders;
using Store.Application.Tests.Fakes;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Application.Tests;

public class GetOrdersHandlerTests
{
    private readonly FakeOrderRepository _orders = new();
    private readonly GetOrdersHandler _handler;

    public GetOrdersHandlerTests()
    {
        _handler = new GetOrdersHandler(_orders);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedOrders()
    {
        await SeedOrders(3);

        var result = await _handler.Handle(new GetOrdersQuery(null, 1, 2));

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
    }

    [Fact]
    public async Task Handle_ShouldFilterByStatus()
    {
        var open = new Order(Guid.NewGuid());
        var closed = new Order(Guid.NewGuid());
        closed.Close();
        await _orders.AddAsync(open);
        await _orders.AddAsync(closed);

        var result = await _handler.Handle(new GetOrdersQuery(OrderStatus.Closed));

        var order = Assert.Single(result.Items);
        Assert.Equal(closed.Id, order.Id);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task Handle_ShouldClampInvalidPaging()
    {
        await SeedOrders(1);

        var result = await _handler.Handle(new GetOrdersQuery(null, 0, 0));

        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalCount);
    }

    private async Task SeedOrders(int count)
    {
        for (var i = 0; i < count; i++)
        {
            await _orders.AddAsync(new Order(Guid.NewGuid()));
        }
    }
}
