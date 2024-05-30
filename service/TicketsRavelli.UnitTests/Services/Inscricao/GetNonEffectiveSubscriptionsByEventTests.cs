using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketsRavelli.API.Controllers.Inscricao;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.UnitTests.Services.Inscricao;

public class GetNonEffectiveSubscriptionsByEventTests
{
    [Fact]
    public async Task GetNonEffectiveSubscriptionsByEvent_ReturnsOkResult() {
        // Arrange
        var idEvent = 1;

        var subscriptions = new List<Subscription> {
            new Subscription(idEvent, "12345678910", 1, "", "", "", "", 1, true, "", false),
            new Subscription(idEvent, "12345678911", 2, "", "", "", "", 1, true, "", false),
            new Subscription(idEvent, "12345678911", 3, "", "", "", "", 1, true, "", false)
        };

        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(s => s.GetNonEffectiveSubscriptionsByEventAsync(idEvent).Result)
            .Returns(subscriptions);

        var logMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, logMock.Object);

        // Act
        var result = await controller.GetNonEffectiveSubscriptionsByEvent(idEvent);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = Assert.IsAssignableFrom<List<Subscription>>(okResult.Value);

        // Verifique se o resultado contém um item com o campo "IdEvento" igual a 1
        Assert.All(resultValue, subscription => Assert.Equal(idEvent, subscription.IdEvento));
        Assert.Equal(subscriptions, resultValue);
    }

    [Fact]
    public async Task GetNonEffectiveSubscriptionsByEvent_ReturnsNotFoundResult() {
        // Arrange
        var idEvent = 2;

        var subscriptions = new List<Subscription>();

        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(s => s.GetNonEffectiveSubscriptionsByEventAsync(idEvent).Result)
            .Returns(subscriptions);

        var logMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, logMock.Object);

        // Act
        var result = await controller.GetNonEffectiveSubscriptionsByEvent(idEvent);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

