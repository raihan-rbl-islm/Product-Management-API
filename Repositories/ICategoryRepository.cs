using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public interface ICategoryRepository
{
    void Add(Category category);
    Category? GetById(int id);
    IEnumerable<ReadCategoryDto> GetAll();
    void SaveChanges();
    void Delete(Category category);
}