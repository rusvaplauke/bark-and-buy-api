using Domain.Constants;
using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly ISellerRepository _sellerRepository;
    private readonly IUserDataClient _userDataClient;

    private const int PendingLifetimeInHours = 2;

    public OrderService(IOrderRepository orderRepository, IStatusRepository statusRepository,
        ISellerRepository sellerRepository, IUserDataClient userDataClient)
    {
        _orderRepository = orderRepository;
        _statusRepository = statusRepository;
        _sellerRepository = sellerRepository;
        _userDataClient = userDataClient;
    }

    public async Task<Order> CreateAsync(CreateOrder order)
    {
        await GetUserNameAsync(order.userId);

        if (await _sellerRepository.GetSellerNameAsync(order.sellerId) is null)
            throw new SellerNotFoundException(order.sellerId);

        var createdOrder = await _orderRepository.CreateOrderAsync(new OrderEntity { SellerId = order.sellerId, UserId = order.userId });

        if (createdOrder is null)
            throw new ErrorCreatingOrderException();

        return await EnrichOrderAsync(createdOrder);
    }

    public async Task<Order> UpdateOrderStatusAsync(int id, int statusId)
    {
        var updatedOrder = await _orderRepository.UpdateOrderStatusAsync(new OrderEntity { Id = id, StatusId = statusId });

        if (updatedOrder is null)
            throw new OrderNotFoundException(id);

        return await EnrichOrderAsync(updatedOrder);
    }

    public async Task<List<Order>> GetAsync(int userId)
    {
        await GetUserNameAsync(userId);

        var result = await _orderRepository.GetOrdersByUserAsync(userId);

        var enrichedOrders = new List<Order>();

        foreach (var order in result)
        {
            enrichedOrders.Add(await EnrichOrderAsync(order));
        }

        return enrichedOrders;
    }

    internal async Task CleanUpExpiredAsync()
    {
        DateTime orderCutoffTime = DateTime.Now.AddHours(-PendingLifetimeInHours);

        await _orderRepository.DeleteExpiredAsync(orderCutoffTime);
    }

    private async Task<Order> EnrichOrderAsync(OrderEntity order)
    {
        int id = order.Id;
        string? status = await _statusRepository.GetStatusValueAsync(order.StatusId);
        string? seller = await _sellerRepository.GetSellerNameAsync(order.SellerId);
        string? userName = await GetUserNameAsync(order.UserId);
        string orderedAt = order.OrderedAt.ToString();

        return new Order(id, status, seller, userName, orderedAt);
    }

    private async Task<string?> GetUserNameAsync(int userId)
    {
        var result = await _userDataClient.GetUserAsync(userId);

        if (!result.IsSuccessful)
            throw new UserNotFoundException(userId, result.ErrorMessage!);

        return result.User.Name;
    }
}
