namespace FulfilmentApi.Domain;

public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Cancelled
}

public class Order
{
    private static readonly Weight MaxWeight = new Weight(30_000, Unit.Grams);

    public Guid Id { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public Address DeliveryAddress { get; private set; }
    public OrderStatus Status { get; private set; }

    public Order(Address deliveryAddress)
    {
        Id = Guid.NewGuid();
        DeliveryAddress = deliveryAddress;
        Status = OrderStatus.Pending;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed.");

        Status = OrderStatus.Processing;
    }

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Items can only be added to pending orders.");

        var totalWeight = _items.Aggregate(new Weight(0, Unit.Grams), (sum, i) => sum + i.Weight) + item.Weight;
        if (totalWeight > MaxWeight)
            throw new InvalidOperationException($"Total weight of order items cannot exceed {MaxWeight}.");

        _items.Add(item);
    }
}
