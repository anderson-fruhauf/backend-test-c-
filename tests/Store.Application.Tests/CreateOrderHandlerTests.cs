using Store.Application.Orders.Commands.CreateOrder;
using Store.Application.Tests.Fakes;

namespace Store.Application.Tests;

public class CreateOrderHandlerTests
{
    private readonly FakeOrderRepository _orders = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _handler = new CreateOrderHandler(_orders, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldPersistOrderAndReturnId()
    {
        var id = await _handler.Handle(new CreateOrderCommand());

        var order = Assert.Single(_orders.Items);
        Assert.Equal(id, order.Id);
        Assert.Equal(1, _unitOfWork.SaveChangesCount);
    }
}
