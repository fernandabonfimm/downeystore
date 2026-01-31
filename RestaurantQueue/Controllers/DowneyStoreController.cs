using Microsoft.AspNetCore.Mvc;
using RestaurantQueue.Models;
using RestaurantQueue.Models.DTOs;
using RestaurantQueue.Services;

namespace RestaurantQueue.Controllers;

[ApiController]
[Route("api/restaurant/[controller]")]
public class DowneyStoreController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPreparationService _preparationService;

    public DowneyStoreController(IOrderService orderService, IPreparationService preparationService)
    {
        _orderService = orderService;
        _preparationService = preparationService;
    }

    [HttpPost("order")]
    public ActionResult<OrderResponse> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            var orderResponse = _orderService.CreateOrder(request);
            _preparationService.StartPreparation(orderResponse.OrderId);
            return CreatedAtAction(nameof(GetOrder), new { id = orderResponse.OrderId }, orderResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("order/{id}")]
    public ActionResult<OrderResponse> GetOrder(int id)
    {
        var order = _orderService.GetOrder(id);
        if (order == null)
            return NotFound(new { error = "Order not found" });

        return Ok(order);
    }

    [HttpGet("orders")]
    public ActionResult<IReadOnlyList<OrderResponse>> GetAllOrders()
    {
        var orders = _orderService.GetAllOrders();
        return Ok(orders);
    }

    [HttpGet("products")]
    public ActionResult<IReadOnlyList<Product>> GetAllProducts()
    {
        var products = _orderService.GetAllProducts();
        return Ok(products);
    }

    [HttpPost("product")]
    public ActionResult<ProductResponse> CreateProduct([FromBody] CreateProductRequest request)
    {
        try
        {
            var productResponse = _orderService.CreateProduct(request);
            return CreatedAtAction(nameof(GetAllProducts), productResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("preparation/grill")]
    public ActionResult<OrderPreparationResponse> UpdateGrill([FromBody] UpdateStationRequest request)
    {
        try
        {
            var result = _preparationService.UpdateStation(request.OrderId, "grill");
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("preparation/salad")]
    public ActionResult<OrderPreparationResponse> UpdateSalad([FromBody] UpdateStationRequest request)
    {
        try
        {
            var result = _preparationService.UpdateStation(request.OrderId, "salad");
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("preparation/fries")]
    public ActionResult<OrderPreparationResponse> UpdateFries([FromBody] UpdateStationRequest request)
    {
        try
        {
            var result = _preparationService.UpdateStation(request.OrderId, "fries");
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("preparation/refill")]
    public ActionResult<OrderPreparationResponse> UpdateRefill([FromBody] UpdateStationRequest request)
    {
        try
        {
            var result = _preparationService.UpdateStation(request.OrderId, "refill");
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("preparation/status/{orderId}")]
    public ActionResult<OrderPreparationResponse> GetPreparationStatus(int orderId)
    {
        var status = _preparationService.GetCurrentStatus(orderId);
        if (status == null)
            return NotFound(new { error = "Preparation status not found for this order" });

        return Ok(status);
    }

    [HttpGet("preparation/history/{orderId}")]
    public ActionResult<IReadOnlyList<OrderPreparationResponse>> GetPreparationHistory(int orderId)
    {
        var history = _preparationService.GetPreparationHistory(orderId);
        return Ok(history);
    }

    [HttpPost("deliver")]
    public ActionResult<DeliverOrderResponse> DeliverOrder([FromBody] DeliverOrderRequest request)
    {
        try
        {
            var order = _orderService.GetOrder(request.OrderId);
            if (order == null)
                return NotFound(new { error = "Order not found" });

            var isReady = _preparationService.IsOrderReady(request.OrderId);
            
            if (!isReady)
            {
                return BadRequest(new { error = "Order is not ready yet. Please complete all preparation stations." });
            }

            var response = new DeliverOrderResponse(
                request.OrderId,
                isReady,
                "Enjoy your meal!",
                DateTime.UtcNow
            );

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}


