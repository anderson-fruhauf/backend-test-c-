using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public Guid Id { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Order(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        Id = id;
        Status = OrderStatus.Open;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(Product product, int quantity)
    {
        EnsureOpen();

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than 0");
        }

        var existing = _items.FirstOrDefault(item => item.ProductId == product.Id);

        if (existing is not null)
        {
            existing.IncreaseQuantity(quantity);
            return;
        }

        _items.Add(new OrderItem(Guid.NewGuid(), product.Id, product.Name, quantity, product.Price));
    }

    public void RemoveItem(Guid productId)
    {
        EnsureOpen();

        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
        {
            throw new InvalidOperationException("Product not found in order");
        }

        _items.Remove(item);
    }

    public void Close()
    {
        EnsureOpen();

        Status = OrderStatus.Closed;
        ClosedAt = DateTime.UtcNow;
    }

    private void EnsureOpen()
    {
        if (Status == OrderStatus.Closed)
        {
            throw new InvalidOperationException("Order already closed");
        }
    }
}