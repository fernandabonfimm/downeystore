using System.Collections.Concurrent;
using RestaurantQueue.Models;
using RestaurantQueue.Utils;

namespace RestaurantQueue.Storage;

public class InMemoryStorage : IStorage
{
    private readonly ConcurrentDictionary<int, Product> _products = new();
    private readonly ConcurrentDictionary<int, Consumer> _consumers = new();
    private readonly ConcurrentDictionary<int, Order> _orders = new();
    private readonly ConcurrentDictionary<int, Payment> _payments = new();
    private readonly ConcurrentBag<OrderPreparation> _orderPreparations = new();

    public InMemoryStorage()
    {
        SeedProducts();
    }

    private void SeedProducts()
    {
        var products = new List<Product>
        {
            new(IdGenerator.NextProductId(), "Big Mac", 5.99m, "Lanche"),
            new(IdGenerator.NextProductId(), "Hamburguer", 2.50m, "Grelha"),
            new(IdGenerator.NextProductId(), "Alface", 0.50m, "Salada"),
            new(IdGenerator.NextProductId(), "Tomate", 0.50m, "Salada"),
            new(IdGenerator.NextProductId(), "Picles", 0.30m, "Salada"),
            new(IdGenerator.NextProductId(), "Batata Media", 2.49m, "Fritas"),
            new(IdGenerator.NextProductId(), "Batata Grande", 3.49m, "Fritas"),
            new(IdGenerator.NextProductId(), "Refil Coca Cola", 1.99m, "Bebida")
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

    public Product? GetProduct(int id)
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

    public Consumer? GetConsumer(int id)
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

    public Order? GetOrder(int id)
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

    public Payment? GetPayment(int id)
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

    public OrderPreparation? GetLatestOrderPreparation(int orderId)
    {
        return _orderPreparations
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.Timestamp)
            .FirstOrDefault();
    }

    public IReadOnlyList<OrderPreparation> GetOrderPreparationHistory(int orderId)
    {
        return _orderPreparations
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.Timestamp)
            .ToList()
            .AsReadOnly();
    }
}
