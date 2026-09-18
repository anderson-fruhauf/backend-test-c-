namespace Store.Application.Orders.Queries.GetOrder;

using Abstractions;

public class GetOrderHandler
{
    private readonly IOrderRepository _orders;

    public GetOrderHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<OrderResponse?> Handle(GetOrderQuery query, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(query.Id, cancellationToken);

        return order is null ? null : OrderResponse.From(order);
    }
}
