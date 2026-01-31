namespace RestaurantQueue.Models.DTOs;

public record DeliverOrderRequest(
    int OrderId
);

public record DeliverOrderResponse(
    int OrderId,
    bool IsReady,
    string Message,
    DateTime DeliveredAt
);

