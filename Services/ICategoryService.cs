using ProductManagementApi.DTOs;

namespace ProductManagementApi.Services;

public interface ICategoryService
{
    ReadCategoryDto CreateCategory(CreateCategoryDto createCategoryDto);
    ReadCategoryDto? GetCategoryById(int id);
    IEnumerable<ReadCategoryDto> GetAllCategories();
    bool UpdateCategory(int id, UpdateCategoryDto updateCategoryDto);
    bool DeleteCategory(int id);
}