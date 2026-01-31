namespace RestaurantQueue.Models.DTOs;

public record DeliverOrderRequest(
    Guid OrderId
);

public record DeliverOrderResponse(
    Guid OrderId,
    bool IsReady,
    string Message,
    DateTime DeliveredAt
);
