using FulfilmentApi.Domain;

public interface IOrderRepository
{
    Task Save(Order order);
    Task<Order?> FindById(Guid orderId);
}

public class OrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();

    public Task Save(Order order)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task<Order?> FindById(Guid orderId)
    {
        _orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }
}
