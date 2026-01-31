using RestaurantQueue.Services;

namespace RestaurantQueue.Tests.Services;

public class DowneyStoreServiceTests
{
    [Fact]
    public void GetMessage_ShouldReturnExpectedMessage()
    {
        // Arrange
        var service = new DowneyStoreService();

        // Act
        var result = service.GetMessage();

        // Assert
        Assert.Equal("Hello from Service!", result);
    }

    [Fact]
    public void GetMessage_ShouldNotReturnNull()
    {
        // Arrange
        var service = new DowneyStoreService();

        // Act
        var result = service.GetMessage();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetMessage_ShouldReturnNonEmptyString()
    {
        // Arrange
        var service = new DowneyStoreService();

        // Act
        var result = service.GetMessage();

        // Assert
        Assert.NotEmpty(result);
    }
}
