using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    IDbConnection _connection;

    public OrderRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<OrderEntity?> CreateOrderAsync(OrderEntity order)
    {
        string sql = "INSERT INTO orders (seller_id, user_id) VALUES (@SellerId, @UserId) RETURNING *;";

        return await _connection.QueryFirstOrDefaultAsync<OrderEntity>(sql, new { SellerId = order.SellerId, UserId = order.UserId });
    }

    public async Task DeleteExpiredAsync(DateTime orderCutoffTime)
    {
        string sql = "DELETE FROM orders WHERE ordered_at < @orderCutoffTime AND status_id = 1;";

        await _connection.QueryAsync(sql, new { orderCutoffTime = orderCutoffTime });
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersByUserAsync(int userId)
    {
        string sql = "SELECT id, status_id, seller_id, user_id, ordered_at FROM orders WHERE user_id = @userId;";

        return await _connection.QueryAsync<OrderEntity>(sql, new { userId = userId });
    }

    public async Task<OrderEntity?> UpdateOrderStatusAsync(OrderEntity order)
    {
        string updateSql = "UPDATE orders SET status_id = @StatusId WHERE id = @Id;";
        string selectSql = "SELECT * FROM orders WHERE id=@Id";

        await _connection.QueryAsync(updateSql, new { StatusId = order.StatusId, Id = order.Id });
        return await _connection.QueryFirstOrDefaultAsync<OrderEntity>(selectSql, new { Id = order.Id });
    }
}
