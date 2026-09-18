namespace Store.Application.Catalog.Queries.GetProducts;

public record GetProductsQuery(string? Name, int Page = 1, int PageSize = 10);
