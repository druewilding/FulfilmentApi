using FulfilmentApi.Domain;

public record CreateOrderRequest(Guid ProductId, int Quantity, Address DeliveryAddress);
