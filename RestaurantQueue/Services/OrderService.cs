using RestaurantQueue.Models;
using RestaurantQueue.Models.DTOs;
using RestaurantQueue.Storage;

namespace RestaurantQueue.Services;

public class OrderService : IOrderService
{
    private readonly IStorage _storage;

    public OrderService(IStorage storage)
    {
        _storage = storage;
    }

    public OrderResponse CreateOrder(CreateOrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ConsumerName))
            throw new ArgumentException("Consumer name is required", nameof(request.ConsumerName));

        if (string.IsNullOrWhiteSpace(request.PaymentMethod))
            throw new ArgumentException("Payment method is required", nameof(request.PaymentMethod));

        if (request.ProductIds == null || request.ProductIds.Count == 0)
            throw new ArgumentException("At least one product is required", nameof(request.ProductIds));

        var products = new List<Product>();
        foreach (var productId in request.ProductIds)
        {
            var product = _storage.GetProduct(productId);
            if (product == null)
                throw new ArgumentException($"Product with ID {productId} not found");
            products.Add(product);
        }

        var totalAmount = products.Sum(p => p.Price);

        var consumer = new Consumer(request.ConsumerName, request.PaymentMethod);
        _storage.AddConsumer(consumer);

        var payment = new Payment(request.PaymentMethod, totalAmount);
        _storage.AddPayment(payment);

        var order = new Order(
            consumer.Id,
            request.ProductIds.AsReadOnly(),
            totalAmount,
            payment.Id
        );
        _storage.AddOrder(order);

        return MapToOrderResponse(order, consumer, products, payment);
    }

    public OrderResponse? GetOrder(Guid orderId)
    {
        var order = _storage.GetOrder(orderId);
        if (order == null)
            return null;

        var consumer = _storage.GetConsumer(order.ConsumerId);
        if (consumer == null)
            return null;

        var products = order.ProductIds
            .Select(id => _storage.GetProduct(id))
            .Where(p => p != null)
            .Cast<Product>()
            .ToList();

        var payment = _storage.GetPayment(order.PaymentId);
        if (payment == null)
            return null;

        return MapToOrderResponse(order, consumer, products, payment);
    }

    public IReadOnlyList<OrderResponse> GetAllOrders()
    {
        var orders = _storage.GetAllOrders();
        var responses = new List<OrderResponse>();

        foreach (var order in orders)
        {
            var orderResponse = GetOrder(order.Id);
            if (orderResponse != null)
                responses.Add(orderResponse);
        }

        return responses.AsReadOnly();
    }

    public IReadOnlyList<Product> GetAllProducts()
    {
        return _storage.GetAllProducts();
    }

    public ProductResponse CreateProduct(CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Product name is required", nameof(request.Name));

        if (request.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero", nameof(request.Price));

        if (string.IsNullOrWhiteSpace(request.Category))
            throw new ArgumentException("Product category is required", nameof(request.Category));

        var validCategories = new[] { "Grelha", "Fritas", "Bebida", "Salada" };
        if (!validCategories.Contains(request.Category, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"Invalid category. Valid categories are: {string.Join(", ", validCategories)}", nameof(request.Category));

        var product = new Product(
            Guid.NewGuid(),
            request.Name,
            request.Price,
            request.Category
        );

        _storage.AddProduct(product);

        return new ProductResponse(
            product.Id,
            product.Name,
            product.Price,
            product.Category,
            DateTime.UtcNow
        );
    }

    private static OrderResponse MapToOrderResponse(Order order, Consumer consumer, List<Product> products, Payment payment)
    {
        var productDetails = products.Select(p => new ProductDetail(
            p.Id,
            p.Name,
            p.Price,
            p.Category
        )).ToList();

        return new OrderResponse(
            order.Id,
            consumer.Name,
            productDetails,
            order.TotalAmount,
            payment.Method,
            order.CreatedAt,
            order.Status
        );
    }
}
