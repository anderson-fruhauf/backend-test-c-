using Store.Domain.Entities;

namespace Store.Application.Catalog.Queries;

public record ProductResponse(Guid Id, string Name, decimal Price)
{
    public static ProductResponse From(Product product) => new(product.Id, product.Name, product.Price);
}
