using Store.Domain.Entities;

namespace Store.Application.Abstractions;

public record ProductSearchResult(IReadOnlyList<Product> Items, int TotalCount);

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductSearchResult> SearchAsync(
        string? name,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
}
