using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Repositories;
using ProductManagementApi.Models;
using FluentAssertions;
using Microsoft.Data.Sqlite;

namespace ProductManagementApi.Tests.Repositories;

public class UserRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly UserRepository _sut;
    private readonly SqliteConnection _connection;

    public UserRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _sut = new UserRepository(_context);
    }

    [Fact]
    public async Task GetByUsernameAsync_WhenUserExists_ShouldReturnUser()
    {
        //Arrange
        var user = new User { Username = "repo-user", Password = "password" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        //Act
        var result = await _sut.GetByUsernameAsync("repo-user");

        //Assertions
        result.Should().NotBeNull();
        result!.Username.Should().Be("repo-user");
    }

    [Fact]
    public async Task Add_ShouldActuallyAddToDatabase()
    {
        //Arrange
        var user = new User { Username = "new-db-user", Password = "password" };

        //Act
        _sut.Add(user);
        await _sut.SaveChangesAsync();

        //Assertions
        var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "new-db-user");
        dbUser.Should().NotBeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
