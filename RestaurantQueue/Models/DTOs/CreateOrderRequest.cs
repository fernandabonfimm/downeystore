namespace RestaurantQueue.Models.DTOs;

public record CreateOrderRequest(
    string ConsumerName,
    string PaymentMethod,
    List<int> ProductIds
);

