namespace FulfilmentApi.UnitTests;

using FulfilmentApi.Domain;
using MassTransit;
using NSubstitute;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateOrder_ShouldReturnOrderResponse()
    {
        // Arrange
        var publishEndpoint = Substitute.For<IPublishEndpoint>();
        var orderService = new OrderService(new OrderRepository(), publishEndpoint);
        var deliveryAddressRequest = new AddressRequest("123 Main St", "12345", "Sample City");
        var orderItemRequest = new OrderItemRequest(Guid.NewGuid(), new WeightRequest(2, Unit.Grams));
        var orderRequest = new CreateOrderRequest([orderItemRequest], deliveryAddressRequest);

        // Act
        var response = await orderService.CreateOrder(orderRequest);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<Order>(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(OrderStatus.Pending, response.Status);
    }

    [Fact]
    public async Task GetOrder_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var publishEndpoint = Substitute.For<IPublishEndpoint>();
        var orderService = new OrderService(new OrderRepository(), publishEndpoint);
        var deliveryAddressRequest = new AddressRequest("123 Main St", "12345", "Sample City");
        var orderItemRequest = new OrderItemRequest(Guid.NewGuid(), new WeightRequest(2, Unit.Grams));
        var orderRequest = new CreateOrderRequest([orderItemRequest], deliveryAddressRequest);
        var response = await orderService.CreateOrder(orderRequest);

        // Act
        var retrievedOrder = await orderService.GetOrder(response.Id);

        // Assert
        Assert.NotNull(retrievedOrder);
        Assert.Equal(new Address("123 Main St", "12345", "Sample City"), retrievedOrder.DeliveryAddress);
    }

    [Fact]
    public async Task GetOrder_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var publishEndpoint = Substitute.For<IPublishEndpoint>();
        var orderService = new OrderService(new OrderRepository(), publishEndpoint);
        var nonExistentOrderId = Guid.NewGuid();

        // Act
        var retrievedOrder = await orderService.GetOrder(nonExistentOrderId);

        // Assert
        Assert.Null(retrievedOrder);
    }
}
