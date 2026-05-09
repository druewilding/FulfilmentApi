public record CreateOrderRequest(List<OrderItemRequest> Items, AddressRequest DeliveryAddress);
