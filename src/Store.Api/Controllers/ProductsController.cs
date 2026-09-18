namespace Store.Api.Controllers;

using Application.Catalog.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly GetProductsHandler _getProducts;

    public ProductsController(GetProductsHandler getProducts)
    {
        _getProducts = getProducts;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? name,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var products = await _getProducts.Handle(
            new GetProductsQuery(name, page, pageSize),
            cancellationToken);
        return Ok(products);
    }
}
