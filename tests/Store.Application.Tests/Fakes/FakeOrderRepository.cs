using Store.Application.Abstractions;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Application.Tests.Fakes;

internal sealed class FakeOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = [];

    public IReadOnlyCollection<Order> Items => _orders.Values;

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _orders.TryGetValue(id, out var order);
        return Task.FromResult(order);
    }

    public Task<OrderSearchResult> SearchAsync(
        OrderStatus? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _orders.Values.AsEnumerable();

        if (status is not null)
        {
            query = query.Where(order => order.Status == status);
        }

        var filtered = query
            .OrderByDescending(order => order.CreatedAt)
            .ToList();

        var items = filtered
            .Skip(skip)
            .Take(take)
            .ToList();

        return Task.FromResult(new OrderSearchResult(items, filtered.Count));
    }
}
