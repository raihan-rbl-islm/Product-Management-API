using FluentAssertions;
using Moq;
using ProductManagementApi.Services;
using ProductManagementApi.Repositories;
using AutoMapper;
using ProductManagementApi.Models;
using ProductManagementApi.Mappings;
using Microsoft.Extensions.Logging;
using ProductManagementApi.DTOs;

namespace ProductManagementApi.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly IMapper _mapper;
    private readonly CategoryService _sut;

    public CategoryServiceTests()
    {
        _categoryRepoMock = new Mock<ICategoryRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        _sut = new CategoryService(_categoryRepoMock.Object, _mapper);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryExists_ShouldReturnReadCategoryDto()
    {
        //Arrange
        int categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Electronics" };
        _categoryRepoMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

        //Act
        var result = await _sut.GetCategoryByIdAsync(categoryId);

        //Assertions
        result.Should().NotBeNull();
        result!.Name.Should().Be(category.Name);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryDoesNotExist_ShouldReturnNull()
    {
        //Arrange
        int categoryId = 99;
        _categoryRepoMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

        //Act
        var result = await _sut.GetCategoryByIdAsync(categoryId);

        //Assertions
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateCategoryAsync_WhenValidRequest_ShouldReturnReadCategoryDto()
    {
        //Arrange
        var createDto = new CreateCategoryDto { Name = "New Category" };

        //Act
        var result = await _sut.CreateCategoryAsync(createDto);

        //Assertions
        result.Should().NotBeNull();
        result.Name.Should().Be(createDto.Name);
        _categoryRepoMock.Verify(repo => repo.Add(It.IsAny<Category>()), Times.Once);
        _categoryRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryExists_ShouldReturnTrue()
    {
        //Arrange
        int categoryId = 1;
        var updateDto = new UpdateCategoryDto { Name = "Updated Category" };
        var category = new Category { Id = categoryId, Name = "Old Category" };
        _categoryRepoMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

        //Act
        var result = await _sut.UpdateCategoryAsync(categoryId, updateDto);

        //Assertions
        result.Should().BeTrue();
        category.Name.Should().Be(updateDto.Name);
        _categoryRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        int categoryId = 99;
        var updateDto = new UpdateCategoryDto { Name = "Updated Category" };
        _categoryRepoMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

        //Act
        var result = await _sut.UpdateCategoryAsync(categoryId, updateDto);

        //Assertions
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryExists_ShouldReturnTrue()
    {
        //Arrange
        int categoryId = 1;
        var category = new Category { Id = categoryId, Name = "To Be Deleted" };
        _categoryRepoMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

        //Act
        var result = await _sut.DeleteCategoryAsync(categoryId);

        //Assertions
        result.Should().BeTrue();
        _categoryRepoMock.Verify(repo => repo.Delete(category), Times.Once);
        _categoryRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        int categoryId = 99;
        _categoryRepoMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

        //Act
        var result = await _sut.DeleteCategoryAsync(categoryId);

        //Assertions
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ShouldReturnCollectionOfReadCategoryDto()
    {
        //Arrange
        var categories = new List<ReadCategoryDto>
        {
            new ReadCategoryDto { Id = 1, Name = "Category 1" }
        };
        _categoryRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

        //Act
        var result = await _sut.GetAllCategoriesAsync();

        //Assertions
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Category 1");
    }
}
