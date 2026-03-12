using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;

namespace ProductManagementApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public ReadCategoryDto CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var category = new Category
        {
            Name = createCategoryDto.Name
        };

        _categoryRepository.Add(category);
        _categoryRepository.SaveChanges();

        return new ReadCategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public ReadCategoryDto? GetCategoryById(int id)
    {
        var category = _categoryRepository.GetById(id);

        if (category == null) return null;

        return new ReadCategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public IEnumerable<ReadCategoryDto> GetAllCategories()
    {
        var categories = _categoryRepository.GetAll();
        return categories.Select(c => new ReadCategoryDto
        {
            Id = c.Id,
            Name = c.Name
        });
    }

    public bool UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
    {
        var category = _categoryRepository.GetById(id);

        if (category == null) return false;

        category.Name = updateCategoryDto.Name;
        _categoryRepository.SaveChanges();

        return true;
    }

    public bool DeleteCategory(int id)
    {
        var category = _categoryRepository.GetById(id);

        if (category == null) return false;

        _categoryRepository.Delete(category);
        _categoryRepository.SaveChanges();
        return true;
    }
}