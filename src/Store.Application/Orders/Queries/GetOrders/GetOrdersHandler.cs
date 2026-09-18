namespace Store.Application.Orders.Queries.GetOrders;

using Abstractions;

public class GetOrdersHandler
{
    private readonly IOrderRepository _orders;

    public GetOrdersHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<IReadOnlyList<OrderResponse>> Handle(
        GetOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        var orders = await _orders.GetAllAsync(cancellationToken);

        return orders.Select(OrderResponse.From).ToList();
    }
}
