using FluentAssertions;
using Moq;
using ProductManagementApi.Services;
using ProductManagementApi.Repositories;
using AutoMapper;
using ProductManagementApi.Models;
using ProductManagementApi.Mappings;
using Microsoft.Extensions.Logging;
using ProductManagementApi.DTOs;

namespace ProductManagementApi.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly IMapper _mapper;

    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        _sut = new ProductService( // System or, Subject, Under Test
            _productRepoMock.Object,
            _categoryRepoMock.Object,
            _mapper
        );
    }

    [Fact]
    public async Task ReadProductAsync_WhenProductExists_ShouldReturnReadProductDto()
    {
        //Arrange
        int productId = 1;
        var product = new Product
        {
            Id = productId,
            Name = "Test Product",
            Price = 10.5m
        };

        _productRepoMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product); // hydrating repo func with key-value (parameter and return val)

        //Act
        var result = await _sut.ReadProductAsync(productId);

        //Assertions
        result.Should().NotBeNull();
        result.Name.Should().Be(product.Name);
        result.Price.Should().Be(product.Price);
    }

    [Fact]
    public async Task ReadProductAsync_WhenProductDoesNotExist_ShouldReturnNull()
    {
        //Arrange
        int productId = 99;
        _productRepoMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        //Act
        var result = await _sut.ReadProductAsync(productId);

        //Assertions
        result.Should().BeNull();
        _productRepoMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
    }

    [Fact]
    public async Task CreateProductAsync_WhenValidRequest_ShouldReturnReadProductDto()
    {
        //Arrange
        var createDto = new CreateProductDto
        {
            Name = "New Product",
            Price = 50.0m,
            CategoryId = 1
        };
        var category = new Category { Id = 1, Name = "Electronics" };

        _categoryRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(category);

        //Act
        var result = await _sut.CreateProductAsync(createDto);

        //Assertions
        result.Should().NotBeNull();
        result.Name.Should().Be(createDto.Name);
        result.Price.Should().Be(createDto.Price);

        _productRepoMock.Verify(repo => repo.Add(It.IsAny<Product>()), Times.Once);
        _productRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateProductAsync_WhenCategoryNotFound_ShouldThrowKeyNotFoundException()
    {
        //Arrange
        var createDto = new CreateProductDto 
        { 
            Name = "Valid Name", 
            Price = 10.0m, 
            CategoryId = 99 
        };
        _categoryRepoMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Category?)null);

        //Act
        var act = () => _sut.CreateProductAsync(createDto);

        //Assertions
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Category with ID 99 not found.");
    }

    [Fact]
    public async Task UpdateProductAsync_WhenProductExists_ShouldReturnTrue()
    {
        //Arrange
        int productId = 1;
        var updateDto = new UpdateProductDto { Price = 25.0m };
        var product = new Product { Id = productId, Name = "Old Name", Price = 10.0m };

        _productRepoMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

        //Act
        var result = await _sut.UpdateProductAsync(productId, updateDto);

        //Assertions
        result.Should().BeTrue();
        product.Price.Should().Be(updateDto.Price);
        _productRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_WhenProductDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        int productId = 99;
        var updateDto = new UpdateProductDto { Price = 20.0m };
        _productRepoMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        //Act
        var result = await _sut.UpdateProductAsync(productId, updateDto);

        //Assertions
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteProductAsync_WhenProductExists_ShouldReturnTrue()
    {
        //Arrange
        int productId = 1;
        var product = new Product { Id = productId, Name = "To Be Deleted" };
        _productRepoMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

        //Act
        var result = await _sut.DeleteProductAsync(productId);

        //Assertions
        result.Should().BeTrue();
        _productRepoMock.Verify(repo => repo.Delete(product), Times.Once);
        _productRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_WhenProductDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        int productId = 99;
        _productRepoMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        //Act
        var result = await _sut.DeleteProductAsync(productId);

        //Assertions
        result.Should().BeFalse();
    }

    [Fact]
    public async Task QueryProductsAsync_ShouldReturnPagedResponse()
    {
        //Arrange
        var queryParams = new ProductQueryParameterDto { PageNumber = 1, PageSize = 10 };
        var productDtos = new List<ReadProductDto>
        {
            new ReadProductDto { Id = 1, Name = "Product 1", Price = 10.0m }
        };

        _productRepoMock.Setup(repo => repo.QueryProductsAsync(queryParams))
            .ReturnsAsync((productDtos, 1));

        //Act
        var result = await _sut.QueryProductsAsync(queryParams);

        //Assertions
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
        result.PageNumber.Should().Be(queryParams.PageNumber);
    }
}