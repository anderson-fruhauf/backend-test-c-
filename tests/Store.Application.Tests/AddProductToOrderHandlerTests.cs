using Store.Application.Orders.Commands.AddProductToOrder;
using Store.Application.Tests.Fakes;
using Store.Domain.Entities;

namespace Store.Application.Tests;

public class AddProductToOrderHandlerTests
{
    private static readonly Guid MouseId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private readonly FakeOrderRepository _orders = new();
    private readonly FakeProductRepository _products = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly AddProductToOrderHandler _handler;

    public AddProductToOrderHandlerTests()
    {
        _products.Add(new Product(MouseId, "Mouse", 100m));
        _handler = new AddProductToOrderHandler(_orders, _products, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldAddItemAndReturnOrder()
    {
        var order = await OpenOrder();

        var result = await _handler.Handle(new AddProductToOrderCommand(order.Id, MouseId, 2));

        Assert.NotNull(result);
        var item = Assert.Single(result.Items);
        Assert.Equal("Mouse", item.ProductName);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(200m, item.Total);
        Assert.Equal(1, _unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnNullWhenOrderIsMissing()
    {
        var result = await _handler.Handle(new AddProductToOrderCommand(Guid.NewGuid(), MouseId, 1));

        Assert.Null(result);
        Assert.Equal(0, _unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnNullWhenProductIsMissing()
    {
        var order = await OpenOrder();

        var result = await _handler.Handle(new AddProductToOrderCommand(order.Id, Guid.NewGuid(), 1));

        Assert.Null(result);
        Assert.Equal(0, _unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task Handle_ShouldFailWhenQuantityIsInvalid()
    {
        var order = await OpenOrder();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _handler.Handle(new AddProductToOrderCommand(order.Id, MouseId, 0)));
    }

    [Fact]
    public async Task Handle_ShouldFailWhenOrderIsClosed()
    {
        var order = await OpenOrder();
        order.Close();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new AddProductToOrderCommand(order.Id, MouseId, 1)));
    }

    private async Task<Order> OpenOrder()
    {
        var order = new Order(Guid.NewGuid());
        await _orders.AddAsync(order);
        return order;
    }
}
