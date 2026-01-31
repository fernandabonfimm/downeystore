namespace RestaurantQueue.Models.DTOs;

public record UpdateStationRequest(
    Guid OrderId,
    string Station
);

public record OrderPreparationResponse(
    Guid Id,
    Guid OrderId,
    bool Grill,
    bool Salad,
    bool Fries,
    bool Refill,
    bool Ready,
    DateTime Timestamp
);
