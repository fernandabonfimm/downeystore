using RestaurantQueue.Models;
using RestaurantQueue.Models.DTOs;
using RestaurantQueue.Storage;
using RestaurantQueue.Utils;

namespace RestaurantQueue.Services;

public class PreparationService : IPreparationService
{
    private readonly IStorage _storage;

    public PreparationService(IStorage storage)
    {
        _storage = storage;
    }

    public OrderPreparationResponse StartPreparation(int orderId)
    {
        var order = _storage.GetOrder(orderId);
        if (order == null)
            throw new ArgumentException($"Order with ID {orderId} not found");

        var preparation = new OrderPreparation(
            IdGenerator.NextPreparationId(),
            orderId,
            grill: false,
            salad: false,
            fries: false,
            refill: false,
            ready: false
        );

        _storage.AddOrderPreparation(preparation);

        return MapToResponse(preparation);
    }

    public OrderPreparationResponse UpdateStation(int orderId, string station)
    {
        var order = _storage.GetOrder(orderId);
        if (order == null)
            throw new ArgumentException($"Order with ID {orderId} not found");

        var currentPreparation = _storage.GetLatestOrderPreparation(orderId);
        if (currentPreparation == null)
        {
            return StartPreparation(orderId);
        }

        var newPreparation = station.ToLower() switch
        {
            "grill" => new OrderPreparation(
                IdGenerator.NextPreparationId(),
                orderId,
                grill: true,
                salad: currentPreparation.Salad,
                fries: currentPreparation.Fries,
                refill: currentPreparation.Refill,
                ready: false
            ),
            "salad" => new OrderPreparation(
                IdGenerator.NextPreparationId(),
                orderId,
                grill: currentPreparation.Grill,
                salad: true,
                fries: currentPreparation.Fries,
                refill: currentPreparation.Refill,
                ready: false
            ),
            "fries" => new OrderPreparation(
                IdGenerator.NextPreparationId(),
                orderId,
                grill: currentPreparation.Grill,
                salad: currentPreparation.Salad,
                fries: true,
                refill: currentPreparation.Refill,
                ready: false
            ),
            "refill" => new OrderPreparation(
                IdGenerator.NextPreparationId(),
                orderId,
                grill: currentPreparation.Grill,
                salad: currentPreparation.Salad,
                fries: currentPreparation.Fries,
                refill: true,
                ready: false
            ),
            _ => throw new ArgumentException($"Invalid station: {station}. Valid stations are: grill, salad, fries, refill")
        };

        var isReady = newPreparation.Grill && 
                      newPreparation.Salad && 
                      newPreparation.Fries && 
                      newPreparation.Refill;

        if (isReady)
        {
            newPreparation = new OrderPreparation(
                IdGenerator.NextPreparationId(),
                orderId,
                grill: newPreparation.Grill,
                salad: newPreparation.Salad,
                fries: newPreparation.Fries,
                refill: newPreparation.Refill,
                ready: true
            );
        }

        _storage.AddOrderPreparation(newPreparation);

        return MapToResponse(newPreparation);
    }

    public OrderPreparationResponse? GetCurrentStatus(int orderId)
    {
        var preparation = _storage.GetLatestOrderPreparation(orderId);
        return preparation == null ? null : MapToResponse(preparation);
    }

    public IReadOnlyList<OrderPreparationResponse> GetPreparationHistory(int orderId)
    {
        var history = _storage.GetOrderPreparationHistory(orderId);
        return history.Select(MapToResponse).ToList().AsReadOnly();
    }

    public bool IsOrderReady(int orderId)
    {
        var latestPreparation = _storage.GetLatestOrderPreparation(orderId);
        return latestPreparation?.Ready ?? false;
    }

    private static OrderPreparationResponse MapToResponse(OrderPreparation preparation)
    {
        return new OrderPreparationResponse(
            preparation.Id,
            preparation.OrderId,
            preparation.Grill,
            preparation.Salad,
            preparation.Fries,
            preparation.Refill,
            preparation.Ready,
            preparation.Timestamp
        );
    }
}
