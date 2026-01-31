namespace RestaurantQueue.Models.DTOs;

public record OrderResponse(
    Guid OrderId,
    string ConsumerName,
    List<ProductDetail> Products,
    decimal TotalAmount,
    string PaymentMethod,
    DateTime CreatedAt,
    string Status
);

public record ProductDetail(
    Guid Id,
    string Name,
    decimal Price,
    string Category
);
