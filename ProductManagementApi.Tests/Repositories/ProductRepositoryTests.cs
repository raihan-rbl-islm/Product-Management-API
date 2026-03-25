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

public class ProductRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ProductRepository _sut;
    private readonly SqliteConnection _connection;

    public ProductRepositoryTests()
    {
        // 1. Setup SQLite In-Memory Connection
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // 2. Configure DbContext to use SQLite
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        // 3. Setup Real AutoMapper
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>(), new LoggerFactory());
        _mapper = config.CreateMapper();

        // 4. Initialize SUT
        _sut = new ProductRepository(_context, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ShouldReturnProduct()
    {
        //Arrange
        var product = new Product { Name = "Repo Test", Price = 10.0m, Stock = 5 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        //Act
        var result = await _sut.GetByIdAsync(product.Id);

        //Assertions
        result.Should().NotBeNull();
        result!.Name.Should().Be("Repo Test");
        result.Price.Should().Be(10.0m);
    }

    [Fact]
    public async Task Add_ShouldActuallyAddToDatabase()
    {
        //Arrange
        var product = new Product { Name = "New DB Product", Price = 15.0m };

        //Act
        _sut.Add(product);
        await _sut.SaveChangesAsync();

        //Assertions
        var dbProduct = await _context.Products.FindAsync(product.Id);
        dbProduct.Should().NotBeNull();
        dbProduct!.Name.Should().Be("New DB Product");
    }

    [Fact]
    public async Task QueryProductsAsync_WhenFilteringByPrice_ShouldReturnMatchingProducts()
    {
        //Arrange
        _context.Products.AddRange(new List<Product>
        {
            new Product { Name = "Cheap", Price = 5.0m },
            new Product { Name = "Mid", Price = 50.0m },
            new Product { Name = "Expensive", Price = 100.0m }
        });
        await _context.SaveChangesAsync();

        var queryParams = new ProductQueryParameterDto { MinPrice = 40.0m, MaxPrice = 60.0m, PageNumber = 1, PageSize = 10 };

        //Act
        var (items, totalCount) = await _sut.QueryProductsAsync(queryParams);

        //Assertions
        items.Should().HaveCount(1);
        items.First().Name.Should().Be("Mid");
        totalCount.Should().Be(1);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
