using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Controllers;
using Xunit;
using Moq;
using Wonderlust.API.Responses.Auth;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Queries.AuthorizeUser;

namespace Wonderlust.API.Tests;

public class AuthControllerTests
{
    private readonly Mock<IMediator> mockMediator;
    private readonly Mock<IMapper> mockMapper;
    private readonly AuthController controller;

    public AuthControllerTests()
    {
        mockMediator = new Mock<IMediator>();
        mockMapper = new Mock<IMapper>();
        controller = new AuthController(mockMediator.Object, mockMapper.Object);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "password" };
        var query = new AuthorizeUserQuery(Email: request.Email, Password: request.Password);
        const string token = "generated_jwt_token";

        mockMapper.Setup(m => m.Map<AuthorizeUserQuery>(request)).Returns(query);
        mockMediator
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>())) // Match any CancellationToken
            .ReturnsAsync(token);

        var result = await controller.Login(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthenticateUserResponse>(okResult.Value);
        Assert.Equal(token, response.Token);

        mockMapper.Verify(m => m.Map<AuthorizeUserQuery>(request), Times.Once);
        mockMediator.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Login_UserNotFound_ReturnsNotFound()
    {
        var request = new LoginRequest { Email = "invalid@example.com", Password = "password" };
        var query = new AuthorizeUserQuery(Email: request.Email, Password: request.Password);
        const string exceptionMessage = "User not found.";

        mockMapper.Setup(m => m.Map<AuthorizeUserQuery>(request)).Returns(query);
        mockMediator
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.Login(request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "wrong_password" };
        var query = new AuthorizeUserQuery(Email: request.Email, Password: request.Password);

        mockMapper.Setup(m => m.Map<AuthorizeUserQuery>(request)).Returns(query);
        mockMediator
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        var result = await controller.Login(request);

        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid credentials", unauthorizedResult.Value);
    }
}