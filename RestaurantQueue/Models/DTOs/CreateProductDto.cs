namespace RestaurantQueue.Models.DTOs;

public record CreateProductRequest(
    string Name,
    decimal Price,
    string Category
);

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    string Category,
    DateTime CreatedAt
);
