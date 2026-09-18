namespace Store.Application.Orders.Commands.RemoveProductFromOrder;

using Abstractions;
using Queries;

public class RemoveProductFromOrderHandler
{
    private readonly IOrderRepository _orders;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveProductFromOrderHandler(IOrderRepository orders, IUnitOfWork unitOfWork)
    {
        _orders = orders;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponse?> Handle(
        RemoveProductFromOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(command.OrderId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        order.RemoveItem(command.ProductId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderResponse.From(order);
    }
}
