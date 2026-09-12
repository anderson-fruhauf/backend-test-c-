namespace Store.Application.Orders.Commands.CreateOrder;

using Abstractions;
using Domain.Entities;

public class CreateOrderHandler
{
    private readonly IOrderRepository _orders;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderHandler(IOrderRepository orders, IUnitOfWork unitOfWork)
    {
        _orders = orders;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        Order order = new Order(Guid.NewGuid());
        await _orders.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}