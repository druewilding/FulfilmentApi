namespace FulfilmentApi.UnitTests;

public class OrderServiceTests
{
    [Fact]
    public void CreateOrder_ShouldReturnOrderResponse()
    {
        // Arrange
        var orderService = new OrderService();
        var order = new Order(Guid.NewGuid(), 2, "123 Main St");

        // Act
        var response = orderService.CreateOrder(order);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<OrderResponse>(response);
        Assert.NotEqual(Guid.Empty, response.OrderId);
        Assert.Equal("pending", response.Status);
    }

    [Fact]
    public void GetOrder_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var orderService = new OrderService();
        var order = new Order(Guid.NewGuid(), 2, "123 Main St");
        var response = orderService.CreateOrder(order);

        // Act
        var retrievedOrder = orderService.GetOrder(response.OrderId);

        // Assert
        Assert.NotNull(retrievedOrder);
        Assert.Equal(order.ProductId, retrievedOrder.ProductId);
        Assert.Equal(order.Quantity, retrievedOrder.Quantity);
        Assert.Equal(order.DeliveryAddress, retrievedOrder.DeliveryAddress);
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
