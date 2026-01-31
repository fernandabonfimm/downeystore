using RestaurantQueue.Models;
using RestaurantQueue.Models.DTOs;

namespace RestaurantQueue.Services;

public interface IOrderService
{
    OrderResponse CreateOrder(CreateOrderRequest request);
    OrderResponse? GetOrder(int orderId);
    IReadOnlyList<OrderResponse> GetAllOrders();
    IReadOnlyList<Product> GetAllProducts();
    ProductResponse CreateProduct(CreateProductRequest request);
}


