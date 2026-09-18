namespace Store.Application.Orders.Commands.CloseOrder;

using Abstractions;
using Queries;

public class CloseOrderHandler
{
    private readonly IOrderRepository _orders;
    private readonly IUnitOfWork _unitOfWork;

    public CloseOrderHandler(IOrderRepository orders, IUnitOfWork unitOfWork)
    {
        _orders = orders;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponse?> Handle(CloseOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(command.OrderId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        order.Close();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderResponse.From(order);
    }
}
