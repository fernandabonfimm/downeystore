namespace RestaurantQueue.Models.DTOs;

public record OrderResponse(
    int OrderId,
    string ConsumerName,
    List<ProductDetail> Products,
    decimal TotalAmount,
    string PaymentMethod,
    DateTime CreatedAt,
    string Status
);

public record ProductDetail(
    int Id,
    string Name,
    decimal Price,
    string Category
);

