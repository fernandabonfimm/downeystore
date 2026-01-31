using RestaurantQueue.Models;
using RestaurantQueue.Services;
using RestaurantQueue.Storage;

namespace RestaurantQueue.Tests.Services;

public class PreparationServiceTests
{
    private readonly IStorage _storage;
    private readonly IPreparationService _preparationService;
    private readonly IOrderService _orderService;

    public PreparationServiceTests()
    {
        _storage = new InMemoryStorage();
        _preparationService = new PreparationService(_storage);
        _orderService = new OrderService(_storage);
    }

    [Fact]
    public void StartPreparation_WithValidOrderId_ShouldCreateInitialPreparation()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);

        // Act
        var result = _preparationService.StartPreparation(order.OrderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(order.OrderId, result.OrderId);
        Assert.False(result.Grill);
        Assert.False(result.Salad);
        Assert.False(result.Fries);
        Assert.False(result.Refill);
        Assert.False(result.Ready);
    }

    [Fact]
    public void StartPreparation_WithInvalidOrderId_ShouldThrowException()
    {
        // Arrange
        var invalidOrderId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _preparationService.StartPreparation(invalidOrderId));
    }

    [Fact]
    public void UpdateStation_Grill_ShouldUpdateGrillStatus()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);
        _preparationService.StartPreparation(order.OrderId);

        // Act
        var result = _preparationService.UpdateStation(order.OrderId, "grill");

        // Assert
        Assert.True(result.Grill);
        Assert.False(result.Salad);
        Assert.False(result.Fries);
        Assert.False(result.Refill);
        Assert.False(result.Ready);
    }

    [Fact]
    public void UpdateStation_MultipleStations_ShouldUpdateSequentially()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);
        _preparationService.StartPreparation(order.OrderId);

        // Act
        var result1 = _preparationService.UpdateStation(order.OrderId, "grill");
        var result2 = _preparationService.UpdateStation(order.OrderId, "salad");
        var result3 = _preparationService.UpdateStation(order.OrderId, "fries");

        // Assert
        Assert.True(result3.Grill);
        Assert.True(result3.Salad);
        Assert.True(result3.Fries);
        Assert.False(result3.Refill);
        Assert.False(result3.Ready);
    }

    [Fact]
    public void UpdateStation_AllStationsComplete_ShouldMarkAsReady()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);
        _preparationService.StartPreparation(order.OrderId);

        // Act
        _preparationService.UpdateStation(order.OrderId, "grill");
        _preparationService.UpdateStation(order.OrderId, "salad");
        _preparationService.UpdateStation(order.OrderId, "fries");
        var finalResult = _preparationService.UpdateStation(order.OrderId, "refill");

        // Assert
        Assert.True(finalResult.Grill);
        Assert.True(finalResult.Salad);
        Assert.True(finalResult.Fries);
        Assert.True(finalResult.Refill);
        Assert.True(finalResult.Ready);
    }

    [Fact]
    public void UpdateStation_WithInvalidStation_ShouldThrowException()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);
        _preparationService.StartPreparation(order.OrderId);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _preparationService.UpdateStation(order.OrderId, "invalid"));
    }

    [Fact]
    public void GetCurrentStatus_ShouldReturnLatestPreparation()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);
        _preparationService.StartPreparation(order.OrderId);
        _preparationService.UpdateStation(order.OrderId, "grill");
        _preparationService.UpdateStation(order.OrderId, "salad");

        // Act
        var result = _preparationService.GetCurrentStatus(order.OrderId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Grill);
        Assert.True(result.Salad);
        Assert.False(result.Fries);
    }

    [Fact]
    public void GetCurrentStatus_WithNonExistentOrder_ShouldReturnNull()
    {
        // Arrange
        var invalidOrderId = Guid.NewGuid();

        // Act
        var result = _preparationService.GetCurrentStatus(invalidOrderId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetPreparationHistory_ShouldReturnAllUpdatesInDescendingOrder()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);
        _preparationService.StartPreparation(order.OrderId);
        _preparationService.UpdateStation(order.OrderId, "grill");
        _preparationService.UpdateStation(order.OrderId, "salad");

        // Act
        var history = _preparationService.GetPreparationHistory(order.OrderId);

        // Assert
        Assert.NotNull(history);
        Assert.Equal(3, history.Count);
        Assert.True(history[0].Timestamp >= history[1].Timestamp);
        Assert.True(history[1].Timestamp >= history[2].Timestamp);
    }

    [Fact]
    public void IsOrderReady_WithCompleteOrder_ShouldReturnTrue()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);
        _preparationService.StartPreparation(order.OrderId);
        _preparationService.UpdateStation(order.OrderId, "grill");
        _preparationService.UpdateStation(order.OrderId, "salad");
        _preparationService.UpdateStation(order.OrderId, "fries");
        _preparationService.UpdateStation(order.OrderId, "refill");

        // Act
        var result = _preparationService.IsOrderReady(order.OrderId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsOrderReady_WithIncompleteOrder_ShouldReturnFalse()
    {
        // Arrange
        var products = _storage.GetAllProducts();
        var productIds = products.Take(1).Select(p => p.Id).ToList();
        var request = new Models.DTOs.CreateOrderRequest("Test User", "Cash", productIds);
        var order = _orderService.CreateOrder(request);
        _preparationService.StartPreparation(order.OrderId);
        _preparationService.UpdateStation(order.OrderId, "grill");

        // Act
        var result = _preparationService.IsOrderReady(order.OrderId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsOrderReady_WithNonExistentOrder_ShouldReturnFalse()
    {
        // Arrange
        var invalidOrderId = Guid.NewGuid();

        // Act
        var result = _preparationService.IsOrderReady(invalidOrderId);

        // Assert
        Assert.False(result);
    }
}
