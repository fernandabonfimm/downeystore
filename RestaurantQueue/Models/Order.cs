namespace RestaurantQueue.Models;

public class Order
{
    public int Id { get; init; }
    public int ConsumerId { get; init; }
    public IReadOnlyList<int> ProductIds { get; init; } = Array.Empty<int>();
    public decimal TotalAmount { get; init; }
    public int PaymentId { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Status { get; init; } = string.Empty;

    public Order()
    {
        Id = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public Order(int id, int consumerId, IReadOnlyList<int> productIds, decimal totalAmount, int paymentId)
    {
        Id = id;
        ConsumerId = consumerId;
        ProductIds = productIds;
        TotalAmount = totalAmount;
        PaymentId = paymentId;
        CreatedAt = DateTime.UtcNow;
        Status = "Pending";
    }
}

