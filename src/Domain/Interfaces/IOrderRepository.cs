using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderEntity>> GetOrdersByUserAsync(int userId);
    Task<OrderEntity?> CreateOrderAsync(OrderEntity order);
    Task<OrderEntity?> UpdateOrderStatusAsync(OrderEntity order);
    Task DeleteExpiredAsync(DateTime orderCutoffTime);
}
