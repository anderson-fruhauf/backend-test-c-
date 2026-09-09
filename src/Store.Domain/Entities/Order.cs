using Store.Domain.Enums;

namespace Store.Domain.Entities;


public class Order
{
    public Guid Id { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }


    public Order(Guid id)
    {
        Id = id;
        Status = OrderStatus.Open;
        CreatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status == OrderStatus.Closed)
        {
            throw new InvalidOperationException("Order already closed");
        }

        Status = OrderStatus.Closed;
        ClosedAt = DateTime.UtcNow;
    }

}