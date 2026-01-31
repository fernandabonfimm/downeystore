using RestaurantQueue.Models;

namespace RestaurantQueue.Storage;

public interface IStorage
{
    void AddProduct(Product product);
    Product? GetProduct(Guid id);
    IReadOnlyList<Product> GetAllProducts();

    void AddConsumer(Consumer consumer);
    Consumer? GetConsumer(Guid id);
    IReadOnlyList<Consumer> GetAllConsumers();

    void AddOrder(Order order);
    Order? GetOrder(Guid id);
    IReadOnlyList<Order> GetAllOrders();

    void AddPayment(Payment payment);
    Payment? GetPayment(Guid id);
    IReadOnlyList<Payment> GetAllPayments();

    void AddOrderPreparation(OrderPreparation preparation);
    OrderPreparation? GetLatestOrderPreparation(Guid orderId);
    IReadOnlyList<OrderPreparation> GetOrderPreparationHistory(Guid orderId);
}
