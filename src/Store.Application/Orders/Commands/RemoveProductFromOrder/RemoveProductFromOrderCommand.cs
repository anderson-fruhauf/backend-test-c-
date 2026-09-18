namespace Store.Application.Orders.Commands.RemoveProductFromOrder;

public record RemoveProductFromOrderCommand(Guid OrderId, Guid ProductId);
