namespace FulfilmentApi.UnitTests;

using FulfilmentApi.Domain;

public class OrderTests
{
    [Fact]
    public void Confirm_ShouldChangeStatusToProcessing_WhenOrderIsPending()
    {
        // Arrange
        var deliveryAddress = new Address("123 Main St", "12345", "Sample City");
        var order = new Order(Guid.NewGuid(), 2, deliveryAddress);
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
        var order = new Order(Guid.NewGuid(), 2, deliveryAddress);
        order.Confirm(); // Change status to Processing
        Assert.Equal(OrderStatus.Processing, order.Status);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.Confirm());
    }
}
