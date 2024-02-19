using Application.Services;
using AutoFixture.Xunit2;
using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BarkAndBuy.UnitTests.Services;

public class OrderServiceTests
{
    private readonly Mock<ISellerRepository> _sellerRepositoryMock;
    private readonly Mock<IStatusRepository> _statusRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IUserDataClient> _userDataClientMock;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _sellerRepositoryMock = new Mock<ISellerRepository>();
        _statusRepositoryMock = new Mock<IStatusRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _userDataClientMock = new Mock<IUserDataClient>();
        _orderService = new OrderService(_orderRepositoryMock.Object,
                                         _statusRepositoryMock.Object,
                                         _sellerRepositoryMock.Object,
                                         _userDataClientMock.Object);
    }
    [Theory]
    [AutoData]
    public async Task CreateAsync_GivenInvalidUserId_ThrowsUserNotFoundException(CreateOrder createOrder)
    {
        // Arrange
        var unsuccessfulResult = new UserDataClientResult { IsSuccessful = false };
        _userDataClientMock.Setup(m => m.GetUserAsync(createOrder.userId)).ReturnsAsync(unsuccessfulResult);

        // Act + Assert
        await _orderService.Invoking(f => f.CreateAsync(createOrder)).Should().ThrowAsync<UserNotFoundException>();
        _userDataClientMock.Verify(m => m.GetUserAsync(createOrder.userId), Times.Once);
    }

    [Theory]
    [AutoData]
    public async Task CreateAsync_GivenValidUserIdAndInvalidSellerId_ThrowsSellerNotFoundException(CreateOrder createOrder)
    {
        // Arrange
        var successfulResult = new UserDataClientResult { IsSuccessful = true };
        _userDataClientMock.Setup(m => m.GetUserAsync(createOrder.userId)).ReturnsAsync(successfulResult);
        _sellerRepositoryMock.Setup(m => m.GetSellerNameAsync(createOrder.sellerId)).ReturnsAsync((string?)null);

        // Act + Assert
        await _orderService.Invoking(f => f.CreateAsync(createOrder)).Should().ThrowAsync<SellerNotFoundException>();
        _userDataClientMock.Verify(m => m.GetUserAsync(createOrder.userId), Times.Once);
        _sellerRepositoryMock.Verify(m => m.GetSellerNameAsync(createOrder.sellerId), Times.Once);
    }

    [Theory]
    [AutoData]
    public async Task CreateAsync_GivenValidIdsWithRepositoryFailure_ThrowsErrorCreatingOrderException(CreateOrder createOrder, string sellerName)
    {
        // Arrange
        var successfulResult = new UserDataClientResult { IsSuccessful = true };

        _userDataClientMock.Setup(m => m.GetUserAsync(createOrder.userId)).ReturnsAsync(successfulResult);
        _sellerRepositoryMock.Setup(m => m.GetSellerNameAsync(createOrder.sellerId)).ReturnsAsync(sellerName);
        _orderRepositoryMock.Setup(m => m.CreateOrderAsync(It.IsAny<OrderEntity>())).ReturnsAsync((OrderEntity?)null);

        // Act + Assert
        await _orderService.Invoking(f => f.CreateAsync(createOrder)).Should().ThrowAsync<ErrorCreatingOrderException>();

        _userDataClientMock.Verify(m => m.GetUserAsync(createOrder.userId), Times.Once);
        _sellerRepositoryMock.Verify(m => m.GetSellerNameAsync(createOrder.sellerId), Times.Once);
        _orderRepositoryMock.Verify(m => m.CreateOrderAsync(It.IsAny<OrderEntity>()), Times.Once);
    }

    [Theory]
    [AutoData]
    public async Task CreateAsync_WithSuccessfulCreateOrder_ReturnsEnrichedOrder(CreateOrder createOrder, string sellerName,
                                                                                OrderEntity orderEntity, string status,
                                                                                UserEntity userEntity)
    {
        // Assert
        orderEntity.UserId = createOrder.userId;
        userEntity.Id = createOrder.userId;
        var successfulResult = new UserDataClientResult { IsSuccessful = true, User = userEntity };

        _userDataClientMock.Setup(m => m.GetUserAsync(createOrder.userId)).ReturnsAsync(successfulResult);
        _sellerRepositoryMock.Setup(m => m.GetSellerNameAsync(createOrder.sellerId)).ReturnsAsync(sellerName);
        _orderRepositoryMock.Setup(m => m.CreateOrderAsync(It.IsAny<OrderEntity>())).ReturnsAsync(orderEntity);
        _statusRepositoryMock.Setup(m => m.GetStatusValueAsync(orderEntity.StatusId)).ReturnsAsync(status);

        // Act
        var result = await _orderService.CreateAsync(createOrder);

        // Assert
        result.Should().NotBeNull();
        await _orderService.Invoking(f => f.CreateAsync(createOrder)).Should().NotThrowAsync();
    }
}