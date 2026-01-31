using RestaurantQueue.Models;
using RestaurantQueue.Models.DTOs;

namespace RestaurantQueue.Services;

public interface IOrderService
{
    OrderResponse CreateOrder(CreateOrderRequest request);
    OrderResponse? GetOrder(Guid orderId);
    IReadOnlyList<OrderResponse> GetAllOrders();
    IReadOnlyList<Product> GetAllProducts();
    ProductResponse CreateProduct(CreateProductRequest request);
}

