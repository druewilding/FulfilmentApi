namespace FulfilmentApi.UnitTests;

using FulfilmentApi.Domain;

public class OrderServiceTests
{
    [Fact]
    public void CreateOrder_ShouldReturnOrderResponse()
    {
        // Arrange
        var orderService = new OrderService();
        var deliveryAddressRequest = new AddressRequest("123 Main St", "12345", "Sample City");
        var orderItemRequest = new OrderItemRequest(Guid.NewGuid(), new WeightRequest(2, Unit.Grams));
        var orderRequest = new CreateOrderRequest([orderItemRequest], deliveryAddressRequest);

        // Act
        var response = orderService.CreateOrder(orderRequest);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<Order>(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(OrderStatus.Pending, response.Status);
    }

    [Fact]
    public void GetOrder_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var orderService = new OrderService();
        var deliveryAddressRequest = new AddressRequest("123 Main St", "12345", "Sample City");
        var orderItemRequest = new OrderItemRequest(Guid.NewGuid(), new WeightRequest(2, Unit.Grams));
        var orderRequest = new CreateOrderRequest([orderItemRequest], deliveryAddressRequest);
        var response = orderService.CreateOrder(orderRequest);

        // Act
        var retrievedOrder = orderService.GetOrder(response.Id);

        // Assert
        Assert.NotNull(retrievedOrder);
        Assert.Equal(new Address("123 Main St", "12345", "Sample City"), retrievedOrder.DeliveryAddress);
    }

    [Fact]
    public void GetOrder_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderService = new OrderService();
        var nonExistentOrderId = Guid.NewGuid();

        // Act
        var retrievedOrder = orderService.GetOrder(nonExistentOrderId);

        // Assert
        Assert.Null(retrievedOrder);
    }
}
