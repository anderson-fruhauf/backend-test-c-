using Store.Application.Abstractions;
using Store.Domain.Entities;

namespace Store.Application.Tests.Fakes;

internal sealed class FakeProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> _products = [];

    public void Add(Product product) => _products[product.Id] = product;

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<ProductSearchResult> SearchAsync(
        string? name,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _products.Values.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var term = name.Trim().ToLower();
            query = query.Where(product => product.Name.ToLower().Contains(term));
        }

        var filtered = query
            .OrderBy(product => product.Name)
            .ToList();

        var items = filtered
            .Skip(skip)
            .Take(take)
            .ToList();

        return Task.FromResult(new ProductSearchResult(items, filtered.Count));
    }
}
