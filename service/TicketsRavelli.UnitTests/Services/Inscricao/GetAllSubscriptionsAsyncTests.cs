using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketsRavelli.API.Controllers.Inscricao;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.UnitTests.Services.Inscricao;

public class GetAllSubscriptionsAsyncTests
{
    [Fact]
    public async Task GetAllSubscriptions_ReturnsOkResult() {
        // Arrange
        var subscriptions = new List<Subscription> {
            new Subscription(1, "12345678910", 1, "", "", "", "", 1, true, "", true),
            new Subscription(2, "12345678911", 2, "", "", "", "", 1, true, "", false),
            new Subscription(3, "12345678911", 3, "", "", "", "", 1, true, "", false)
        };

        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(st => st.GetAllSubscriptionsAsync().Result).Returns(subscriptions);

        var logMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, logMock.Object);

        // Act
        var result = await controller.GetAllSubscriptions();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = Assert.IsAssignableFrom<IEnumerable<Subscription>>(okResult.Value);
        Assert.Equal(subscriptions, resultValue);
    }

    [Fact]
    public async Task GetSubscriptionById_SubscriptionNotFound_ReturnsNotFoundResult() {
        // Arrange
        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(st => st.GetAllSubscriptionsAsync()).ReturnsAsync(new List<Subscription>());

        var loggerMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetAllSubscriptions();

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}