namespace RestaurantQueue.Models;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Category { get; init; } = string.Empty;

    public Product()
    {
        Id = Guid.NewGuid();
    }

    public Product(Guid id, string name, decimal price, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }
}
