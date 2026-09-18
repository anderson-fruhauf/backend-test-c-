namespace Store.Application.Orders.Commands.AddProductToOrder;

using Abstractions;
using Queries;

public class AddProductToOrderHandler
{
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductToOrderHandler(
        IOrderRepository orders,
        IProductRepository products,
        IUnitOfWork unitOfWork)
    {
        _orders = orders;
        _products = products;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponse?> Handle(
        AddProductToOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(command.OrderId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        var product = await _products.GetByIdAsync(command.ProductId, cancellationToken);
        if (product is null)
        {
            return null;
        }

        order.AddItem(product, command.Quantity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderResponse.From(order);
    }
}
