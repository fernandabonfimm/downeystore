using RestaurantQueue.Models;

namespace RestaurantQueue.Storage;

public interface IStorage
{
    void AddProduct(Product product);
    Product? GetProduct(int id);
    IReadOnlyList<Product> GetAllProducts();

    void AddConsumer(Consumer consumer);
    Consumer? GetConsumer(int id);
    IReadOnlyList<Consumer> GetAllConsumers();

    void AddOrder(Order order);
    Order? GetOrder(int id);
    IReadOnlyList<Order> GetAllOrders();

    void AddPayment(Payment payment);
    Payment? GetPayment(int id);
    IReadOnlyList<Payment> GetAllPayments();

    void AddOrderPreparation(OrderPreparation preparation);
    OrderPreparation? GetLatestOrderPreparation(int orderId);
    IReadOnlyList<OrderPreparation> GetOrderPreparationHistory(int orderId);
}

