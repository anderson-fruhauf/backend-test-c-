namespace Store.Api.Controllers;

using Application.Orders.Commands.AddProductToOrder;
using Application.Orders.Commands.CloseOrder;
using Application.Orders.Commands.CreateOrder;
using Application.Orders.Commands.RemoveProductFromOrder;
using Application.Orders.Queries.GetOrder;
using Application.Orders.Queries.GetOrders;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _createOrder;
    private readonly AddProductToOrderHandler _addProduct;
    private readonly RemoveProductFromOrderHandler _removeProduct;
    private readonly CloseOrderHandler _closeOrder;
    private readonly GetOrderHandler _getOrder;
    private readonly GetOrdersHandler _getOrders;

    public OrdersController(
        CreateOrderHandler createOrder,
        AddProductToOrderHandler addProduct,
        RemoveProductFromOrderHandler removeProduct,
        CloseOrderHandler closeOrder,
        GetOrderHandler getOrder,
        GetOrdersHandler getOrders)
    {
        _createOrder = createOrder;
        _addProduct = addProduct;
        _removeProduct = removeProduct;
        _closeOrder = closeOrder;
        _getOrder = getOrder;
        _getOrders = getOrders;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var id = await _createOrder.Handle(new CreateOrderCommand(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> AddItem(
        Guid id,
        AddItemRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _addProduct.Handle(
            new AddProductToOrderCommand(id, request.ProductId, request.Quantity),
            cancellationToken);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpDelete("{id:guid}/items/{productId:guid}")]
    public async Task<IActionResult> RemoveItem(
        Guid id,
        Guid productId,
        CancellationToken cancellationToken)
    {
        var order = await _removeProduct.Handle(
            new RemoveProductFromOrderCommand(id, productId),
            cancellationToken);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        var order = await _closeOrder.Handle(new CloseOrderCommand(id), cancellationToken);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] OrderStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var orders = await _getOrders.Handle(new GetOrdersQuery(status, page, pageSize), cancellationToken);
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var order = await _getOrder.Handle(new GetOrderQuery(id), cancellationToken);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }
}

public record AddItemRequest(Guid ProductId, int Quantity);
