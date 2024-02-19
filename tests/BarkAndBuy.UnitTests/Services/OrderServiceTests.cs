using Application.Services;
using AutoFixture;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBuy.UnitTests.Services;

public class OrderServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ISellerRepository> _sellerRepositoryMock;
    private readonly Mock<IStatusRepository> _statusRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IUserDataClient> _userDataClientMock;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _fixture = new Fixture();
        _sellerRepositoryMock = new Mock<ISellerRepository>();
        _statusRepositoryMock = new Mock<IStatusRepository> ();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _userDataClientMock = new Mock<IUserDataClient> ();
        _orderService = new OrderService(_orderRepositoryMock.Object, 
                                         _statusRepositoryMock.Object, 
                                         _sellerRepositoryMock.Object, 
                                         _userDataClientMock.Object);
    }

    [Fact]
    public async Task aa()
    {
        // Arrange
        
        // Act

        // Assert
    }
}
