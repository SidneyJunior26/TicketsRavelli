using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketsRavelli.API.Controllers.Inscricao;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.UnitTests.Services.Inscricao;

public class GetSubscriptionsByEventTests {
    [Fact]
    public async Task GetSubscriptionsByEvent_SubscriptionFound_ReturnsOkResult() {
        // Arrange
        var idEvent = 1;
        var subscriptions = new List<Subscription>
        {
            new Subscription(idEvent, "12345678910", 1, "", "", "", "", 1, true, "", true),
            new Subscription(idEvent, "12345678911", 2, "", "", "", "", 1, true, "", false),
            new Subscription(idEvent, "12345678912", 3, "", "", "", "", 1, true, "", false)
        };

        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(st => st.GetSubscriptionsByEventAsync(idEvent))
            .ReturnsAsync(subscriptions);

        var loggerMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetSubscriptionsByEvent(idEvent);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = Assert.IsAssignableFrom<List<Subscription>>(okResult.Value);

        // Verifique se o resultado contém um item com o campo "IdEvento" igual a 1
        Assert.All(resultValue, subscription => Assert.Equal(idEvent, subscription.IdEvento));
        Assert.Equal(subscriptions, resultValue);
    }

    [Fact]
    public async Task GetSubscriptionsByEvent_SubscriptionNotFound_ReturnsNotFoundResult() {
        // Arrange
        var idEvent = 2;
        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(st => st.GetSubscriptionsByEventAsync(idEvent)).ReturnsAsync(new List<Subscription>());

        var loggerMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetSubscriptionsByEvent(idEvent);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

