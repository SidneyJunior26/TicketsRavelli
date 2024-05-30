using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketsRavelli.API.Controllers.Inscricao;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.UnitTests.Services.Inscricao;

public class CheckIfAthleteSubscribedByEventTests
{
    [Fact]
    public async Task CheckIfAthleteSubscribedByEvent_AthleteExists_ReturnOkResult() {
        // Arrange
        var cpfAtlhete = "01234567890";
        var idEvent = 1;

        bool userExists = true;

        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(s => s.CheckIfAthleteSubscribedByEventAsync(cpfAtlhete, idEvent)).ReturnsAsync(userExists);

        var loggerMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.CheckIfAthleteSubscribedByEvent(cpfAtlhete, idEvent);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CheckIfAthleteSubscribedByEvent_AthleteNotExists_ReturnNotFoundResult() {
        // Arrange
        var cpfAtlhete = "01234567890";
        var idEvent = 1;

        bool userExists = false;

        var subscriptionServiceMock = new Mock<ISubscriptionService>();
        subscriptionServiceMock.Setup(s => s.CheckIfAthleteSubscribedByEventAsync(cpfAtlhete, idEvent)).ReturnsAsync(userExists);

        var loggerMock = new Mock<ILogSystem>();

        var controller = new SubscriptionController(subscriptionServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.CheckIfAthleteSubscribedByEvent(cpfAtlhete, idEvent);

        Assert.IsType<NotFoundResult>(result);
    }
}