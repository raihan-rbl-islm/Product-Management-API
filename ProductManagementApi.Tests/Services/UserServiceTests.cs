using FluentAssertions;
using Moq;
using ProductManagementApi.Services;
using ProductManagementApi.Repositories;
using AutoMapper;
using ProductManagementApi.Models;
using ProductManagementApi.Mappings;
using Microsoft.Extensions.Logging;
using ProductManagementApi.DTOs;
using ProductManagementApi.Enums;

namespace ProductManagementApi.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly IMapper _mapper;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        _sut = new UserService(_userRepoMock.Object, _mapper);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenCredentialsValid_ShouldReturnReadUserDto()
    {
        //Arrange
        var loginDto = new LoginRequestDto { Username = "test", Password = "password" };
        var user = new User { Username = "test", Password = "password" };
        _userRepoMock.Setup(repo => repo.GetByUsernameAsync(loginDto.Username)).ReturnsAsync(user);

        //Act
        var result = await _sut.AuthenticateAsync(loginDto);

        //Assertions
        result.Should().NotBeNull();
        result!.Username.Should().Be(user.Username);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenCredentialsInvalid_ShouldReturnNull()
    {
        //Arrange
        var loginDto = new LoginRequestDto { Username = "test", Password = "wrongpassword" };
        var user = new User { Username = "test", Password = "password" };
        _userRepoMock.Setup(repo => repo.GetByUsernameAsync(loginDto.Username)).ReturnsAsync(user);

        //Act
        var result = await _sut.AuthenticateAsync(loginDto);

        //Assertions
        result.Should().BeNull();
    }

    [Fact]
    public async Task RegisterAsync_WhenUsernameIsAvailable_ShouldReturnReadUserDto()
    {
        //Arrange
        var registerDto = new RegisterRequestDto { Username = "newuser", Password = "password", Role = RoleEnum.User };
        _userRepoMock.Setup(repo => repo.GetByUsernameAsync(registerDto.Username)).ReturnsAsync((User?)null);

        //Act
        var result = await _sut.RegisterAsync(registerDto);

        //Assertions
        result.Should().NotBeNull();
        result!.Username.Should().Be(registerDto.Username);
        _userRepoMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
        _userRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WhenUsernameExists_ShouldReturnNull()
    {
        //Arrange
        var registerDto = new RegisterRequestDto { Username = "existing", Password = "password", Role = RoleEnum.User };
        var existingUser = new User { Username = "existing" };
        _userRepoMock.Setup(repo => repo.GetByUsernameAsync(registerDto.Username)).ReturnsAsync(existingUser);

        //Act
        var result = await _sut.RegisterAsync(registerDto);

        //Assertions
        result.Should().BeNull();
        _userRepoMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
    }
}
