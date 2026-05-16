namespace FulfilmentApi.Domain;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}

public class OrderPlaced : IDomainEvent
{
    public Guid OrderId { get; }
    public DateTimeOffset OccurredAt { get; }

    public OrderPlaced(Guid orderId)
    {
        OrderId = orderId;
        OccurredAt = DateTimeOffset.UtcNow;
    }
}

public class OrderConfirmed : IDomainEvent
{
    public Guid OrderId { get; }
    public DateTimeOffset OccurredAt { get; }

    public OrderConfirmed(Guid orderId)
    {
        OrderId = orderId;
        OccurredAt = DateTimeOffset.UtcNow;
    }
}
