namespace Store.Application.Orders.Queries.GetOrders;

using Domain.Enums;

public record GetOrdersQuery(OrderStatus? Status, int Page = 1, int PageSize = 10);
