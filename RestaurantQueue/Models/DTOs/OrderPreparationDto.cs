namespace RestaurantQueue.Models.DTOs;

public record UpdateStationRequest(
    int OrderId,
    string Station
);

public record OrderPreparationResponse(
    int Id,
    int OrderId,
    bool Grill,
    bool Salad,
    bool Fries,
    bool Refill,
    bool Ready,
    DateTime Timestamp
);

