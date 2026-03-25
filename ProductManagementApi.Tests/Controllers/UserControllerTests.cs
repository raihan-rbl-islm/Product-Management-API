using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagementApi.Controllers;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;
using FluentAssertions;
using ProductManagementApi.Enums;

namespace ProductManagementApi.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly UserController _sut;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _sut = new UserController(_tokenServiceMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task Login_WhenValidCredentials_ShouldReturn200OkWithToken()
    {
        //Arrange
        var loginDto = new LoginRequestDto { Username = "testuser", Password = "password" };
        var readUserDto = new ReadUserDTO { Username = "testuser", Role = RoleEnum.User };
        _userServiceMock.Setup(s => s.AuthenticateAsync(loginDto)).ReturnsAsync(readUserDto);
        _tokenServiceMock.Setup(s => s.GenerateJwtToken(readUserDto)).Returns("mock-token");

        //Act
        var result = await _sut.Login(loginDto);

        //Assertions
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(new { token = "mock-token" });
    }

    [Fact]
    public async Task Login_WhenInvalidCredentials_ShouldReturn401Unauthorized()
    {
        //Arrange
        var loginDto = new LoginRequestDto { Username = "invalid", Password = "wrong" };
        _userServiceMock.Setup(s => s.AuthenticateAsync(loginDto)).ReturnsAsync((ReadUserDTO?)null);

        //Act
        var result = await _sut.Login(loginDto);

        //Assertions
        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task Register_WhenUsernameDoesNotExist_ShouldReturn200OkWithToken()
    {
        //Arrange
        var registerDto = new RegisterRequestDto { Username = "newuser", Password = "password", Role = RoleEnum.User };
        var readUserDto = new ReadUserDTO { Username = "newuser", Role = RoleEnum.User };
        _userServiceMock.Setup(s => s.RegisterAsync(registerDto)).ReturnsAsync(readUserDto);
        _tokenServiceMock.Setup(s => s.GenerateJwtToken(readUserDto)).Returns("mock-token");

        //Act
        var result = await _sut.Register(registerDto);

        //Assertions
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(new { token = "mock-token" });
    }

    [Fact]
    public async Task Register_WhenUsernameExists_ShouldReturn400BadRequest()
    {
        //Arrange
        var registerDto = new RegisterRequestDto { Username = "existinguser", Password = "password", Role = RoleEnum.User };
        _userServiceMock.Setup(s => s.RegisterAsync(registerDto)).ReturnsAsync((ReadUserDTO?)null);

        //Act
        var result = await _sut.Register(registerDto);

        //Assertions
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.Value.Should().Be("Username already exists.");
    }
}
