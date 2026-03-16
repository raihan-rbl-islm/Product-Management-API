using ProductManagementApi.DTOs;

namespace ProductManagementApi.Services;

public interface ICategoryService
{
    Task<ReadCategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<ReadCategoryDto?> GetCategoryByIdAsync(int id);
    Task<IEnumerable<ReadCategoryDto>> GetAllCategoriesAsync();
    Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
    Task<bool> DeleteCategoryAsync(int id);
}