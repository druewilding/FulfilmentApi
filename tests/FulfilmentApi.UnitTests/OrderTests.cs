namespace FulfilmentApi.UnitTests;

using FulfilmentApi.Domain;

public class OrderTests
{
    [Fact]
    public void Confirm_ShouldChangeStatusToProcessing_WhenOrderIsPending()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 2, "123 Main St");
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
        var order = new Order(Guid.NewGuid(), 2, "123 Main St");
        order.Confirm(); // Change status to Processing
        Assert.Equal(OrderStatus.Processing, order.Status);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.Confirm());
    }
}
