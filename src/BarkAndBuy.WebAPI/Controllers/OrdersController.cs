using Application.Services;
using Domain.Constants;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BarkAndBuy.WebAPI.Controllers;

///<summary>
///Controller for managing order information
///</summary>
///
[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    ///<summary>
    ///Create controller with order service injection
    ///</summary>
    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    ///<summary>
    ///Create new order with user, seller id 
    ///</summary>
    /// <returns>Returns the created order details.</returns>
    [SwaggerOperation("CreateOrder")]
    [SwaggerResponse(StatusCodes.Status201Created, "Successfully created new order.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User or seller not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrder order)
    {
        var createdOrder = await _orderService.CreateAsync(order);
        return Created(nameof(Create), createdOrder);
    }

    ///<summary>
    ///Change order status to "delivered"
    ///</summary>
    /// <returns>Returns the updated order details.</returns>
    [SwaggerOperation("DeliverOrder")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully changed order status to \"delivered\".")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
    [HttpPut("{id}/deliver")]
    public async Task<IActionResult> Deliver(int id)
    {
        return Ok(await _orderService.UpdateOrderStatusAsync(id, StatusIds.Delivered));
    }

    ///<summary>
    ///Change order status to "completed"
    ///</summary>
    /// <returns>Returns the updated order details.</returns>
    [SwaggerOperation("CompletedOrder")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully changed order status to \"completed\".")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        return Ok(await _orderService.UpdateOrderStatusAsync(id, StatusIds.Completed));
    }

    ///<summary>
    ///Get all orders for selected user
    ///</summary>
    /// <returns>Returns a list of orders for selected user.</returns>
    [SwaggerOperation("GetOrdersByUser")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved list of orders for user.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int userId)
    {
        return Ok(await _orderService.GetAsync(userId));
    }
}