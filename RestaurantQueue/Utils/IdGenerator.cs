namespace RestaurantQueue.Utils;

public static class IdGenerator
{
    private static int _productId = 0;
    private static int _consumerId = 0;
    private static int _orderId = 0;
    private static int _paymentId = 0;
    private static int _preparationId = 0;

    public static int NextProductId() => Interlocked.Increment(ref _productId);
    public static int NextConsumerId() => Interlocked.Increment(ref _consumerId);
    public static int NextOrderId() => Interlocked.Increment(ref _orderId);
    public static int NextPaymentId() => Interlocked.Increment(ref _paymentId);
    public static int NextPreparationId() => Interlocked.Increment(ref _preparationId);

    public static void Reset()
    {
        _productId = 0;
        _consumerId = 0;
        _orderId = 0;
        _paymentId = 0;
        _preparationId = 0;
    }
}
