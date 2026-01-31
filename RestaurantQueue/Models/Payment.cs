namespace RestaurantQueue.Models;

public class Payment
{
    public Guid Id { get; init; }
    public string Method { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime ProcessedAt { get; init; }

    public Payment()
    {
        Id = Guid.NewGuid();
        ProcessedAt = DateTime.UtcNow;
    }

    public Payment(string method, decimal amount)
    {
        Id = Guid.NewGuid();
        Method = method;
        Amount = amount;
        ProcessedAt = DateTime.UtcNow;
    }
}
