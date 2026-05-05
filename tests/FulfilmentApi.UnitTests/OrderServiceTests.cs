namespace FulfilmentApi.UnitTests;

using FulfilmentApi.Domain;

public class OrderServiceTests
{
    [Fact]
    public void CreateOrder_ShouldReturnOrderResponse()
    {
        // Arrange
        var orderService = new OrderService();
        var order = new Order(Guid.NewGuid(), 2, "123 Main St");

        // Act
        var response = orderService.CreateOrder(new CreateOrderRequest(order.ProductId, order.Quantity, order.DeliveryAddress));

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
        var order = new Order(Guid.NewGuid(), 2, "123 Main St");
        var response = orderService.CreateOrder(new CreateOrderRequest(order.ProductId, order.Quantity, order.DeliveryAddress));

        // Act
        var retrievedOrder = orderService.GetOrder(response.Id);

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
