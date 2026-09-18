namespace Store.Api.Controllers;

using Application.Orders.Commands.CreateOrder;
using Application.Orders.Queries.GetOrder;
using Application.Orders.Queries.GetOrders;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _createOrder;
    private readonly GetOrderHandler _getOrder;
    private readonly GetOrdersHandler _getOrders;

    public OrdersController(
        CreateOrderHandler createOrder,
        GetOrderHandler getOrder,
        GetOrdersHandler getOrders)
    {
        _createOrder = createOrder;
        _getOrder = getOrder;
        _getOrders = getOrders;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var id = await _createOrder.Handle(new CreateOrderCommand(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _getOrders.Handle(new GetOrdersQuery(), cancellationToken);
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
