using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    internal async Task CleanUpExpired()
    {
        DateTime orderCutoffTime = DateTime.Now.AddHours(-2);

        // TODO: determine order time until which orders need to be deleted
        // TODO: call Delete with this time

        // await _orderRepository.DeleteExpired(orderCutoffTime);
    }
}
