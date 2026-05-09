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

        foreach (var itemRequest in orderRequest.Items)
        {
            var orderItem = new OrderItem(itemRequest.ProductId, new Weight(itemRequest.Weight.Amount, itemRequest.Weight.Unit));
            order.AddItem(orderItem);
        }

        var orderId = order.Id;
        orders[orderId] = order;
        Console.WriteLine($"Order created with ID: {orderId}, containing {order.Items.Count} items, total weight: {order.Items.Sum(i => i.Weight.Amount)} {order.Items.FirstOrDefault()?.Weight.Unit}");
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
