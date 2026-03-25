using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagementApi.Controllers;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;
using FluentAssertions;

namespace ProductManagementApi.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<ICategoryService> _serviceMock;
    private readonly CategoriesController _sut;

    public CategoriesControllerTests()
    {
        _serviceMock = new Mock<ICategoryService>();
        _sut = new CategoriesController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetCategory_WhenCategoryExists_ShouldReturn200Ok()
    {
        //Arrange
        int categoryId = 1;
        var categoryDto = new ReadCategoryDto { Id = categoryId, Name = "Electronics" };
        _serviceMock.Setup(s => s.GetCategoryByIdAsync(categoryId)).ReturnsAsync(categoryDto);

        //Act
        var result = await _sut.GetCategory(categoryId);

        //Assertions
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(categoryDto);
    }

    [Fact]
    public async Task GetCategory_WhenCategoryDoesNotExist_ShouldReturn404NotFound()
    {
        //Arrange
        int categoryId = 99;
        _serviceMock.Setup(s => s.GetCategoryByIdAsync(categoryId)).ReturnsAsync((ReadCategoryDto?)null);

        //Act
        var result = await _sut.GetCategory(categoryId);

        //Assertions
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateCategory_WhenValid_ShouldReturn200Ok()
    {
        //Arrange
        var createDto = new CreateCategoryDto { Name = "New Category" };
        var readDto = new ReadCategoryDto { Id = 1, Name = "New Category" };
        _serviceMock.Setup(s => s.CreateCategoryAsync(createDto)).ReturnsAsync(readDto);

        //Act
        var result = await _sut.CreateCategory(createDto);

        //Assertions
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(readDto);
    }

    [Fact]
    public async Task UpdateCategory_WhenCategoryExists_ShouldReturn204NoContent()
    {
        //Arrange
        int categoryId = 1;
        var updateDto = new UpdateCategoryDto { Name = "Updated Category" };
        _serviceMock.Setup(s => s.UpdateCategoryAsync(categoryId, updateDto)).ReturnsAsync(true);

        //Act
        var result = await _sut.UpdateCategory(categoryId, updateDto);

        //Assertions
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateCategory_WhenCategoryDoesNotExist_ShouldReturn404NotFound()
    {
        //Arrange
        int categoryId = 99;
        var updateDto = new UpdateCategoryDto { Name = "Updated Category" };
        _serviceMock.Setup(s => s.UpdateCategoryAsync(categoryId, updateDto)).ReturnsAsync(false);

        //Act
        var result = await _sut.UpdateCategory(categoryId, updateDto);

        //Assertions
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteCategory_WhenCategoryExists_ShouldReturn204NoContent()
    {
        //Arrange
        int categoryId = 1;
        _serviceMock.Setup(s => s.DeleteCategoryAsync(categoryId)).ReturnsAsync(true);

        //Act
        var result = await _sut.DeleteCategory(categoryId);

        //Assertions
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteCategory_WhenCategoryDoesNotExist_ShouldReturn404NotFound()
    {
        //Arrange
        int categoryId = 99;
        _serviceMock.Setup(s => s.DeleteCategoryAsync(categoryId)).ReturnsAsync(false);

        //Act
        var result = await _sut.DeleteCategory(categoryId);

        //Assertions
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturn200OkWithCategories()
    {
        //Arrange
        var categories = new List<ReadCategoryDto>
        {
            new ReadCategoryDto { Id = 1, Name = "Electronics" }
        };
        _serviceMock.Setup(s => s.GetAllCategoriesAsync()).ReturnsAsync(categories);

        //Act
        var result = await _sut.GetAllCategories();

        //Assertions
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(categories);
    }
}
