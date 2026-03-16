using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public interface ICategoryRepository
{
    void Add(Category category);
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<ReadCategoryDto>> GetAllAsync();
    Task SaveChangesAsync();
    void Delete(Category category);
}