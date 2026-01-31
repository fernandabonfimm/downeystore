namespace RestaurantQueue.Models;

public class Order
{
    public Guid Id { get; init; }
    public Guid ConsumerId { get; init; }
    public IReadOnlyList<Guid> ProductIds { get; init; } = Array.Empty<Guid>();
    public decimal TotalAmount { get; init; }
    public Guid PaymentId { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Status { get; init; } = string.Empty;

    public Order()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public Order(Guid consumerId, IReadOnlyList<Guid> productIds, decimal totalAmount, Guid paymentId)
    {
        Id = Guid.NewGuid();
        ConsumerId = consumerId;
        ProductIds = productIds;
        TotalAmount = totalAmount;
        PaymentId = paymentId;
        CreatedAt = DateTime.UtcNow;
        Status = "Pending";
    }
}
