public record CreateOrderRequest(Guid ProductId, int Quantity, AddressRequest DeliveryAddress);
