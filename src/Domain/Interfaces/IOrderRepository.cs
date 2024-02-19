using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderEntity>> GetOrdersByUserAsync(int userId);
    Task<OrderEntity?> CreateOrder(OrderEntity order);
    Task<OrderEntity?> UpdateOrder(OrderEntity order);
    Task DeleteExpired(DateTime orderCutoffTime);
}
