namespace RestaurantQueue.Models;

public class OrderPreparation
{
    public int Id { get; init; }
    public int OrderId { get; init; }
    public bool Grill { get; init; }
    public bool Salad { get; init; }
    public bool Fries { get; init; }
    public bool Refill { get; init; }
    public bool Ready { get; init; }
    public DateTime Timestamp { get; init; }

    public OrderPreparation()
    {
        Id = 0;
        Timestamp = DateTime.UtcNow;
    }

    public OrderPreparation(int id, int orderId, bool grill, bool salad, bool fries, bool refill, bool ready)
    {
        Id = id;
        OrderId = orderId;
        Grill = grill;
        Salad = salad;
        Fries = fries;
        Refill = refill;
        Ready = ready;
        Timestamp = DateTime.UtcNow;
    }
}

