namespace Store.Application.Orders.Queries;

using Domain.Entities;

public record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal Total);

public record OrderResponse(
    Guid Id,
    string Status,
    DateTime CreatedAt,
    DateTime? ClosedAt,
    IReadOnlyList<OrderItemResponse> Items)
{
    public static OrderResponse From(Order order) => new(
        order.Id,
        order.Status.ToString(),
        order.CreatedAt,
        order.ClosedAt,
        order.Items
            .Select(item => new OrderItemResponse(
                item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Quantity,
                item.Total))
            .ToList());
}
