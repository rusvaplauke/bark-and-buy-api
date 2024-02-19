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

        var createdOrder = await _orderRepository.CreateOrder(new OrderEntity { SellerId = order.sellerId, UserId = order.userId });

        if (createdOrder is null)
            throw new ErrorCreatingOrderException();

        return await EnrichOrder(createdOrder);
    }

    public async Task<Order> DeliverAsync(int id)
    {
        var updatedOrder = await _orderRepository.UpdateOrder(new OrderEntity { Id = id, StatusId = 3 }); //TODO: retrieve status value, not hardcode

        if (updatedOrder is null)
            throw new OrderNotFoundException(id);

        return await EnrichOrder(updatedOrder);
    }

    public async Task<Order> CompleteAsync(int id)
    {
        //TODO: only allow to complete orders that were deivered

        var updatedOrder = await _orderRepository.UpdateOrder(new OrderEntity { Id = id, StatusId = 4 }); //TODO: retrieve status value, not hardcode

        if (updatedOrder is null)
            throw new OrderNotFoundException(id);

        return await EnrichOrder(updatedOrder);
    }

    public async Task<List<Order>> GetAsync(int userId)
    {
        await GetUserNameAsync(userId);

        var result = await _orderRepository.GetOrdersByUserAsync(userId);

        var enrichedOrders = new List<Order>();

        foreach (var order in result)
        {
            enrichedOrders.Add(await EnrichOrder(order));
        }

        return enrichedOrders;
    }

    internal async Task CleanUpExpired()
    {
        DateTime orderCutoffTime = DateTime.Now.AddHours(-2);

        await _orderRepository.DeleteExpired(orderCutoffTime);
    }

    private async Task<Order> EnrichOrder(OrderEntity order)
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
