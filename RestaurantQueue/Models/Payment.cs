namespace RestaurantQueue.Models;

public class Payment
{
    public int Id { get; init; }
    public string Method { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime ProcessedAt { get; init; }

    public Payment()
    {
        Id = 0;
        ProcessedAt = DateTime.UtcNow;
    }

    public Payment(int id, string method, decimal amount)
    {
        Id = id;
        Method = method;
        Amount = amount;
        ProcessedAt = DateTime.UtcNow;
    }
}

