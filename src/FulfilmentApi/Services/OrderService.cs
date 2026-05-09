using FulfilmentApi.Domain;

public interface IOrderService
{
    Order CreateOrder(CreateOrderRequest orderRequest);
    Order? GetOrder(Guid orderId);
}

public class OrderService : IOrderService
{
    private readonly Dictionary<Guid, Order> orders = new();

    public Order CreateOrder(CreateOrderRequest orderRequest)
    {
        var deliveryAddress = new Address(orderRequest.DeliveryAddress.Street, orderRequest.DeliveryAddress.PostalCode, orderRequest.DeliveryAddress.City);
        var order = new Order(deliveryAddress);
        var orderId = order.Id;
        orders[orderId] = order;
        return order;
    }

    public Order? GetOrder(Guid orderId)
    {
        if (orders.TryGetValue(orderId, out var order))
        {
            return order;
        }
        return null;
    }
}
