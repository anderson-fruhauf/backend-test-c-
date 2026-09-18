using Store.Application.Abstractions;

namespace Store.Application.Catalog.Queries.GetProducts;

public class GetProductsHandler
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;

    private readonly IProductRepository _products;

    public GetProductsHandler(IProductRepository products)
    {
        _products = products;
    }

    public async Task<PagedResponse<ProductResponse>> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken = default)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1
            ? DefaultPageSize
            : Math.Min(query.PageSize, MaxPageSize);

        var skip = (page - 1) * pageSize;
        var result = await _products.SearchAsync(
            query.Name,
            skip,
            pageSize,
            cancellationToken);

        return new PagedResponse<ProductResponse>(
            result.Items.Select(ProductResponse.From).ToList(),
            page,
            pageSize,
            result.TotalCount);
    }
}
