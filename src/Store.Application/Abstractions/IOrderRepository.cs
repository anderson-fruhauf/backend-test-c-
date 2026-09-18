using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Application.Abstractions;

public record OrderSearchResult(IReadOnlyList<Order> Items, int TotalCount);

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrderSearchResult> SearchAsync(
        OrderStatus? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
}
