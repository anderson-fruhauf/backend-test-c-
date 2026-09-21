using Store.Application.Catalog.Queries.GetProducts;
using Store.Application.Tests.Fakes;
using Store.Domain.Entities;

namespace Store.Application.Tests;

public class GetProductsHandlerTests
{
    private readonly FakeProductRepository _products = new();
    private readonly GetProductsHandler _handler;

    public GetProductsHandlerTests()
    {
        _products.Add(new Product(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Notebook", 3500m));
        _products.Add(new Product(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Mouse", 100m));
        _products.Add(new Product(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Teclado", 200m));
        _products.Add(new Product(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Monitor", 1200m));
        _handler = new GetProductsHandler(_products);
    }

    [Fact]
    public async Task Handle_ShouldFilterByName()
    {
        var result = await _handler.Handle(new GetProductsQuery("mo"));

        Assert.Equal(2, result.TotalCount);
        Assert.Collection(
            result.Items,
            product => Assert.Equal("Monitor", product.Name),
            product => Assert.Equal("Mouse", product.Name));
    }

    [Fact]
    public async Task Handle_ShouldClampPageSize()
    {
        var result = await _handler.Handle(new GetProductsQuery(null, 1, 100));

        Assert.Equal(50, result.PageSize);
        Assert.Equal(4, result.Items.Count);
    }
}
