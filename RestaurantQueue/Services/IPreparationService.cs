using RestaurantQueue.Models;
using RestaurantQueue.Models.DTOs;

namespace RestaurantQueue.Services;

public interface IPreparationService
{
    OrderPreparationResponse StartPreparation(Guid orderId);
    OrderPreparationResponse UpdateStation(Guid orderId, string station);
    OrderPreparationResponse? GetCurrentStatus(Guid orderId);
    IReadOnlyList<OrderPreparationResponse> GetPreparationHistory(Guid orderId);
    bool IsOrderReady(Guid orderId);
}

