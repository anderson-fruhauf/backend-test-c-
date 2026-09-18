using Microsoft.EntityFrameworkCore;
using Store.Application.Abstractions;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) => _context = context;

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

    public async Task<ProductSearchResult> SearchAsync(
        string? name,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var term = name.Trim().ToLower();
            query = query.Where(product => product.Name.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(product => product.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new ProductSearchResult(items, totalCount);
    }
}
