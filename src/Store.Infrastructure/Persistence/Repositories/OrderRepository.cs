using Microsoft.EntityFrameworkCore;
using Store.Application.Abstractions;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context) => _context = context;

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Orders
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);

    public async Task<OrderSearchResult> SearchAsync(
        OrderStatus? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Orders
            .Include(order => order.Items)
            .AsQueryable();

        if (status is not null)
        {
            query = query.Where(order => order.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(order => order.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new OrderSearchResult(items, totalCount);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        => await _context.Orders.AddAsync(order, cancellationToken);
}
