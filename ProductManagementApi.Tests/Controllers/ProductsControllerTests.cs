using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManagementApi.Controllers;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;
using FluentAssertions;

namespace ProductManagementApi.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _serviceMock;
    private readonly Mock<ILogger<ProductsController>> _loggerMock;
    private readonly ProductsController _sut;

    public ProductsControllerTests()
    {
        _serviceMock = new Mock<IProductService>();
        _loggerMock = new Mock<ILogger<ProductsController>>();

        _sut = new ProductsController(_serviceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ReadProduct_WhenProductExists_ShouldReturn200Ok()
    {
        //Arrange
        int productId = 1;
        var productDto = new ReadProductDto { Id = productId, Name = "Controller Test", Price = 10.0m };
        _serviceMock.Setup(s => s.ReadProductAsync(productId)).ReturnsAsync(productDto);

        //Act
        var result = await _sut.ReadProduct(productId);

        //Assertions
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(productDto);
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task ReadProduct_WhenProductDoesNotExist_ShouldReturn404NotFound()
    {
        //Arrange
        int productId = 99;
        _serviceMock.Setup(s => s.ReadProductAsync(productId)).ReturnsAsync((ReadProductDto?)null);

        //Act
        var result = await _sut.ReadProduct(productId);

        //Assertions
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateProduct_WhenValid_ShouldReturn200Ok()
    {
        //Arrange
        var createDto = new CreateProductDto { Name = "New", Price = 10.0m };
        var readDto = new ReadProductDto { Id = 1, Name = "New", Price = 10.0m };
        _serviceMock.Setup(s => s.CreateProductAsync(createDto)).ReturnsAsync(readDto);

        //Act
        var result = await _sut.CreateProduct(createDto);

        //Assertions
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(readDto);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductExists_ShouldReturn204NoContent()
    {
        //Arrange
        int productId = 1;
        var updateDto = new UpdateProductDto { Price = 20.0m };
        _serviceMock.Setup(s => s.UpdateProductAsync(productId, updateDto)).ReturnsAsync(true);

        //Act
        var result = await _sut.UpdateProduct(productId, updateDto);

        //Assertions
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ShouldReturn404NotFound()
    {
        //Arrange
        int productId = 99;
        var updateDto = new UpdateProductDto { Price = 20.0m };
        _serviceMock.Setup(s => s.UpdateProductAsync(productId, updateDto)).ReturnsAsync(false);

        //Act
        var result = await _sut.UpdateProduct(productId, updateDto);

        //Assertions
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task QueryProducts_ShouldReturn200OkWithProducts()
    {
        //Arrange
        var queryParams = new ProductQueryParameterDto { PageNumber = 1, PageSize = 10 };
        var pagedResponse = new PagedResponse<ReadProductDto>
        {
            Items = new List<ReadProductDto>
            {
                new ReadProductDto { Id = 1, Name = "Test Product", Price = 10.0m }
            },
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 10
        };

        _serviceMock.Setup(s => s.QueryProductsAsync(queryParams)).ReturnsAsync(pagedResponse);

        //Act
        var result = await _sut.QueryProducts(queryParams);

        //Assertions
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(pagedResponse);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ShouldReturn204NoContent()
    {
        //Arrange
        int productId = 1;
        _serviceMock.Setup(s => s.DeleteProductAsync(productId)).ReturnsAsync(true);

        //Act
        var result = await _sut.DeleteProduct(productId);

        //Assertions
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ShouldReturn404NotFound()
    {
        //Arrange
        int productId = 99;
        _serviceMock.Setup(s => s.DeleteProductAsync(productId)).ReturnsAsync(false);

        //Act
        var result = await _sut.DeleteProduct(productId);

        //Assertions
        result.Should().BeOfType<NotFoundResult>();
    }
}
