using System.Collections.Concurrent;
using RestaurantQueue.Models;

namespace RestaurantQueue.Storage;

public class InMemoryStorage : IStorage
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();
    private readonly ConcurrentDictionary<Guid, Consumer> _consumers = new();
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();
    private readonly ConcurrentDictionary<Guid, Payment> _payments = new();
    private readonly ConcurrentBag<OrderPreparation> _orderPreparations = new();

    public InMemoryStorage()
    {
        SeedProducts();
    }

    private void SeedProducts()
    {
        var products = new List<Product>
        {
            new(Guid.NewGuid(), "Big Mac", 5.99m, "Grelha"),
            new(Guid.NewGuid(), "Batata Media", 2.49m, "Fritas"),
            new(Guid.NewGuid(), "Batata Grande", 3.49m, "Fritas"),
            new(Guid.NewGuid(), "Refil Coca Cola", 1.99m, "Bebida")
        };

        foreach (var product in products)
        {
            _products.TryAdd(product.Id, product);
        }
    }

    public void AddProduct(Product product)
    {
        _products.TryAdd(product.Id, product);
    }

    public Product? GetProduct(Guid id)
    {
        _products.TryGetValue(id, out var product);
        return product;
    }

    public IReadOnlyList<Product> GetAllProducts()
    {
        return _products.Values.ToList().AsReadOnly();
    }

    public void AddConsumer(Consumer consumer)
    {
        _consumers.TryAdd(consumer.Id, consumer);
    }

    public Consumer? GetConsumer(Guid id)
    {
        _consumers.TryGetValue(id, out var consumer);
        return consumer;
    }

    public IReadOnlyList<Consumer> GetAllConsumers()
    {
        return _consumers.Values.ToList().AsReadOnly();
    }

    public void AddOrder(Order order)
    {
        _orders.TryAdd(order.Id, order);
    }

    public Order? GetOrder(Guid id)
    {
        _orders.TryGetValue(id, out var order);
        return order;
    }

    public IReadOnlyList<Order> GetAllOrders()
    {
        return _orders.Values.ToList().AsReadOnly();
    }

    public void AddPayment(Payment payment)
    {
        _payments.TryAdd(payment.Id, payment);
    }

    public Payment? GetPayment(Guid id)
    {
        _payments.TryGetValue(id, out var payment);
        return payment;
    }

    public IReadOnlyList<Payment> GetAllPayments()
    {
        return _payments.Values.ToList().AsReadOnly();
    }

    public void AddOrderPreparation(OrderPreparation preparation)
    {
        _orderPreparations.Add(preparation);
    }

    public OrderPreparation? GetLatestOrderPreparation(Guid orderId)
    {
        return _orderPreparations
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.Timestamp)
            .FirstOrDefault();
    }

    public IReadOnlyList<OrderPreparation> GetOrderPreparationHistory(Guid orderId)
    {
        return _orderPreparations
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.Timestamp)
            .ToList()
            .AsReadOnly();
    }
}
