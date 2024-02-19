using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarkAndBuy.WebAPI.Controllers;

[ApiController]
[Route("movies")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    
}
