using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Orders.Queries;
using Store.Infrastructure.Persistence;

namespace Store.Api.Tests;

public class OrdersApiTests : IClassFixture<StoreApiFactory>
{
    private readonly HttpClient _client;

    public OrdersApiTests(StoreApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldReturn201()
    {
        var response = await _client.PostAsync("/api/orders", null);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<CreatedOrderResponse>();
        Assert.NotEqual(Guid.Empty, created!.Id);
        Assert.Equal($"/api/orders/{created.Id}", response.Headers.Location?.AbsolutePath);
    }

    [Fact]
    public async Task GetById_ShouldReturn404WhenMissing()
    {
        var response = await _client.GetAsync($"/api/orders/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddItem_ShouldReturn200()
    {
        var orderId = await CreateOrder();

        var response = await _client.PostAsJsonAsync(
            $"/api/orders/{orderId}/items",
            new { productId = ProductSeed.MouseId, quantity = 2 });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var order = await response.Content.ReadFromJsonAsync<OrderResponse>();
        var item = Assert.Single(order!.Items);
        Assert.Equal(ProductSeed.MouseId, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(200m, item.Total);
    }

    [Fact]
    public async Task AddItem_ShouldReturn400WhenQuantityIsInvalid()
    {
        var orderId = await CreateOrder();

        var response = await _client.PostAsJsonAsync(
            $"/api/orders/{orderId}/items",
            new { productId = ProductSeed.MouseId, quantity = 0 });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal("Invalid argument", problem!.Title);
    }

    [Fact]
    public async Task Close_ShouldReturn409WhenAlreadyClosed()
    {
        var orderId = await CreateOrder();

        var first = await _client.PostAsync($"/api/orders/{orderId}/close", null);
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);

        var second = await _client.PostAsync($"/api/orders/{orderId}/close", null);

        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);

        var problem = await second.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal("Invalid operation", problem!.Title);
        Assert.Equal("Order already closed", problem.Detail);
    }

    private async Task<Guid> CreateOrder()
    {
        var response = await _client.PostAsync("/api/orders", null);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<CreatedOrderResponse>();
        return created!.Id;
    }

    private sealed record CreatedOrderResponse(Guid Id);
}
