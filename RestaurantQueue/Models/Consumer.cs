namespace RestaurantQueue.Models;

public class Consumer
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PaymentMethod { get; init; } = string.Empty;

    public Consumer()
    {
        Id = Guid.NewGuid();
    }

    public Consumer(string name, string paymentMethod)
    {
        Id = Guid.NewGuid();
        Name = name;
        PaymentMethod = paymentMethod;
    }
}
