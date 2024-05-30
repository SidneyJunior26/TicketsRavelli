using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketsRavelli.API.Controllers.Inscricao;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.UnitTests.Services.Inscricao;

public class GetSubscriptionByIdTests
{
    [Fact]
    public async Task GetSubscriptionById_SubscriptionFound_ReturnsOkResult() {
        // Arrange
        var subscription = new Subscription(1, "12345678910", 1, "", "", "", "", 1, true, "", true);

        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(st => st.GetSubscriptionByIdAsync(1)).ReturnsAsync(subscription);

        var loggerMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetSubscriptionById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = Assert.IsAssignableFrom<Subscription>(okResult.Value);
        Assert.Equal(subscription, resultValue);
    }

    [Fact]
    public async Task GetSubscriptionById_SubscriptionNotFound_ReturnsNotFoundResult() {
        // Arrange
        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(st => st.GetSubscriptionByIdAsync(1)).ReturnsAsync((Subscription)null);

        var loggerMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetSubscriptionById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

