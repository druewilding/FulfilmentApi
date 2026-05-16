namespace FulfilmentApi.UnitTests;

using FulfilmentApi.Domain;

public class OrderTests
{
    [Fact]
    public void Confirm_ShouldChangeStatusToProcessing_WhenOrderIsPending()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");
        var order = new Order(deliveryAddress);
        Assert.Equal(OrderStatus.Pending, order.Status);

        // Act
        order.Confirm();

        // Assert
        Assert.Equal(OrderStatus.Processing, order.Status);
    }

    [Fact]
    public void Confirm_ShouldThrowInvalidOperationException_WhenOrderIsNotPending()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");
        var order = new Order(deliveryAddress);
        order.Confirm(); // Change status to Processing
        Assert.Equal(OrderStatus.Processing, order.Status);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.Confirm());
    }

    [Fact]
    public void Order_ShouldStartWithNoOrderItems()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");
        var order = new Order(deliveryAddress);

        // Act & Assert
        Assert.Empty(order.Items);
    }

    [Fact]
    public void Order_AddItem_ShouldAddOrderItemToOrder()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");
        var order = new Order(deliveryAddress);
        var productId = Guid.NewGuid();
        var weight = new Weight(500, Unit.Grams);
        var orderItem = new OrderItem(productId, weight);

        // Act
        order.AddItem(orderItem);

        // Assert
        Assert.Single(order.Items);
        Assert.Equal(productId, order.Items[0].ProductId);
        Assert.Equal(weight, order.Items[0].Weight);
    }

    [Fact]
    public void Order_AddItem_ShouldThrowInvalidOperationException_WhenTotalWeightExceedsLimit()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");
        var order = new Order(deliveryAddress);
        var productId = Guid.NewGuid();
        var weight1 = new Weight(20_000, Unit.Grams);
        var weight2 = new Weight(15_000, Unit.Grams);
        var orderItem1 = new OrderItem(productId, weight1);
        var orderItem2 = new OrderItem(productId, weight2);

        // Act
        order.AddItem(orderItem1);

        // Assert
        Assert.Throws<InvalidOperationException>(() => order.AddItem(orderItem2));
    }

    [Fact]
    public void Order_AddItem_CannotAddItem_WhenOrderIsNotPending()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");
        var order = new Order(deliveryAddress);
        var productId = Guid.NewGuid();
        var weight = new Weight(500, Unit.Grams);
        var orderItem = new OrderItem(productId, weight);
        order.Confirm(); // Change status to Processing

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.AddItem(orderItem));
    }

    [Fact]
    public void Order_ShouldRaiseOrderPlacedEvent_WhenCreated()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");

        // Act
        var order = new Order(deliveryAddress);

        // Assert
        Assert.Contains(order.DomainEvents, e => e is OrderPlaced placed && placed.OrderId == order.Id);
        Assert.NotEqual(default, ((OrderPlaced)order.DomainEvents.First(e => e is OrderPlaced)).OccurredAt);
    }

    [Fact]
    public void Order_ShouldRaiseOrderConfirmedEvent_WhenConfirmed()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");
        var order = new Order(deliveryAddress);

        // Act
        order.Confirm();

        // Assert
        Assert.Contains(order.DomainEvents, e => e is OrderConfirmed confirmed && confirmed.OrderId == order.Id);
        Assert.NotEqual(default, ((OrderConfirmed)order.DomainEvents.First(e => e is OrderConfirmed)).OccurredAt);
    }
}
