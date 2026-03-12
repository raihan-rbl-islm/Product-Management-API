using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public interface ICategoryRepository
{
    void Add(Category category);
    Category? GetById(int id);
    IEnumerable<Category> GetAll();
    void SaveChanges();
    void Delete(Category category);
}