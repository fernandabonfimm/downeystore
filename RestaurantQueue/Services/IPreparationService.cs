using RestaurantQueue.Models;
using RestaurantQueue.Models.DTOs;

namespace RestaurantQueue.Services;

public interface IPreparationService
{
    OrderPreparationResponse StartPreparation(int orderId);
    OrderPreparationResponse UpdateStation(int orderId, string station);
    OrderPreparationResponse? GetCurrentStatus(int orderId);
    IReadOnlyList<OrderPreparationResponse> GetPreparationHistory(int orderId);
    bool IsOrderReady(int orderId);
}


