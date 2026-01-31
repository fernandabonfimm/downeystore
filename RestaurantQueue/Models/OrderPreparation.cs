namespace RestaurantQueue.Models;

public class OrderPreparation
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public bool Grill { get; init; }
    public bool Salad { get; init; }
    public bool Fries { get; init; }
    public bool Refill { get; init; }
    public bool Ready { get; init; }
    public DateTime Timestamp { get; init; }

    public OrderPreparation()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
    }

    public OrderPreparation(Guid orderId, bool grill, bool salad, bool fries, bool refill, bool ready)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Grill = grill;
        Salad = salad;
        Fries = fries;
        Refill = refill;
        Ready = ready;
        Timestamp = DateTime.UtcNow;
    }
}
