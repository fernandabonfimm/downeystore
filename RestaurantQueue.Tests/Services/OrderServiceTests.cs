using RestaurantQueue.Models;
using RestaurantQueue.Models.DTOs;
using RestaurantQueue.Services;
using RestaurantQueue.Storage;

namespace RestaurantQueue.Tests.Services;

public class OrderServiceTests
{
    private readonly IStorage _storage;
    private readonly IOrderService _orderService;

    public OrderServiceTests()
    {
        _storage = new InMemoryStorage();
        _orderService = new OrderService(_storage);
    }

    [Fact]
    public void CreateOrder_WithValidData_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(2).Select(p => p.Id).ToList();
        
        var request = new CreateOrderRequest(
            "John Doe",
            "Credit Card",
            productIds
        );

        // Act
        var result = _orderService.CreateOrder(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.ConsumerName);
        Assert.Equal("Credit Card", result.PaymentMethod);
        Assert.Equal(2, result.Products.Count);
        Assert.True(result.TotalAmount > 0);
        Assert.Equal("Pending", result.Status);
    }

    [Fact]
    public void CreateOrder_WithoutConsumerName_ShouldThrowException()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        
        var request = new CreateOrderRequest(
            "",
            "Credit Card",
            productIds
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _orderService.CreateOrder(request));
    }

    [Fact]
    public void CreateOrder_WithoutProducts_ShouldThrowException()
    {
        // Arrange
        var request = new CreateOrderRequest(
            "John Doe",
            "Credit Card",
            new List<Guid>()
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _orderService.CreateOrder(request));
    }

    [Fact]
    public void CreateOrder_WithInvalidProductId_ShouldThrowException()
    {
        // Arrange
        var request = new CreateOrderRequest(
            "John Doe",
            "Credit Card",
            new List<Guid> { Guid.NewGuid() }
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _orderService.CreateOrder(request));
    }

    [Fact]
    public void GetOrder_WithValidId_ShouldReturnOrder()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        
        var request = new CreateOrderRequest(
            "Jane Smith",
            "Debit Card",
            productIds
        );
        var createdOrder = _orderService.CreateOrder(request);

        // Act
        var result = _orderService.GetOrder(createdOrder.OrderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdOrder.OrderId, result.OrderId);
        Assert.Equal("Jane Smith", result.ConsumerName);
    }

    [Fact]
    public void GetOrder_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = _orderService.GetOrder(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAllOrders_ShouldReturnAllCreatedOrders()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        
        var request1 = new CreateOrderRequest("Customer 1", "Cash", productIds);
        var request2 = new CreateOrderRequest("Customer 2", "Credit Card", productIds);
        
        _orderService.CreateOrder(request1);
        _orderService.CreateOrder(request2);

        // Act
        var result = _orderService.GetAllOrders();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
    }

    [Fact]
    public void GetAllProducts_ShouldReturnAllProducts()
    {
        // Act
        var result = _orderService.GetAllProducts();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count > 0);
    }

    [Fact]
    public void CreateOrder_ShouldCalculateTotalAmountCorrectly()
    {
        // Arrange
        var products = _storage.GetAllProducts().Take(3).ToList();
        var productIds = products.Select(p => p.Id).ToList();
        var expectedTotal = products.Sum(p => p.Price);
        
        var request = new CreateOrderRequest(
            "Test Customer",
            "Credit Card",
            productIds
        );

        // Act
        var result = _orderService.CreateOrder(request);

        // Assert
        Assert.Equal(expectedTotal, result.TotalAmount);
    }

    [Fact]
    public void CreateProduct_WithValidData_ShouldCreateProductSuccessfully()
    {
        // Arrange
        var request = new CreateProductRequest(
            "McChicken",
            4.99m,
            "Grelha"
        );

        // Act
        var result = _orderService.CreateProduct(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("McChicken", result.Name);
        Assert.Equal(4.99m, result.Price);
        Assert.Equal("Grelha", result.Category);
    }

    [Fact]
    public void CreateProduct_WithoutName_ShouldThrowException()
    {
        // Arrange
        var request = new CreateProductRequest(
            "",
            4.99m,
            "Grelha"
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _orderService.CreateProduct(request));
    }

    [Fact]
    public void CreateProduct_WithZeroPrice_ShouldThrowException()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Test Product",
            0m,
            "Grelha"
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _orderService.CreateProduct(request));
    }

    [Fact]
    public void CreateProduct_WithNegativePrice_ShouldThrowException()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Test Product",
            -5.99m,
            "Grelha"
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _orderService.CreateProduct(request));
    }

    [Fact]
    public void CreateProduct_WithoutCategory_ShouldThrowException()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Test Product",
            4.99m,
            ""
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _orderService.CreateProduct(request));
    }

    [Fact]
    public void CreateProduct_WithInvalidCategory_ShouldThrowException()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Test Product",
            4.99m,
            "Sobremesa"
        );

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _orderService.CreateProduct(request));
        Assert.Contains("Invalid category", exception.Message);
    }

    [Fact]
    public void CreateProduct_ShouldBeAddedToStorage()
    {
        // Arrange
        var initialCount = _storage.GetAllProducts().Count;
        var request = new CreateProductRequest(
            "Quarter Pounder",
            6.99m,
            "Grelha"
        );

        // Act
        var result = _orderService.CreateProduct(request);
        var finalCount = _storage.GetAllProducts().Count;

        // Assert
        Assert.Equal(initialCount + 1, finalCount);
        var addedProduct = _storage.GetProduct(result.Id);
        Assert.NotNull(addedProduct);
        Assert.Equal("Quarter Pounder", addedProduct.Name);
    }
}

