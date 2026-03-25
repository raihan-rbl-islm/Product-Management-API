using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Repositories;
using ProductManagementApi.Models;
using ProductManagementApi.DTOs;
using AutoMapper;
using ProductManagementApi.Mappings;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace ProductManagementApi.Tests.Repositories;

public class CategoryRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly CategoryRepository _sut;
    private readonly SqliteConnection _connection;

    public CategoryRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>(), new LoggerFactory());
        _mapper = config.CreateMapper();

        _sut = new CategoryRepository(_context, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCategoryExists_ShouldReturnCategory()
    {
        //Arrange
        var category = new Category { Name = "Electronics" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        //Act
        var result = await _sut.GetByIdAsync(category.Id);

        //Assertions
        result.Should().NotBeNull();
        result!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task Add_ShouldActuallyAddToDatabase()
    {
        //Arrange
        var category = new Category { Name = "New Category" };

        //Act
        _sut.Add(category);
        await _sut.SaveChangesAsync();

        //Assertions
        var dbCategory = await _context.Categories.FindAsync(category.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be("New Category");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        //Arrange
        _context.Categories.AddRange(new List<Category>
        {
            new Category { Name = "Cat 1" },
            new Category { Name = "Cat 2" }
        });
        await _context.SaveChangesAsync();

        //Act
        var result = await _sut.GetAllAsync();

        //Assertions
        result.Should().HaveCount(2);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
