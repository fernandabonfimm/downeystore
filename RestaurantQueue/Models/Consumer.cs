namespace RestaurantQueue.Models;

public class Consumer
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PaymentMethod { get; init; } = string.Empty;

    public Consumer()
    {
        Id = 0;
    }

    public Consumer(int id, string name, string paymentMethod)
    {
        Id = id;
        Name = name;
        PaymentMethod = paymentMethod;
    }
}

