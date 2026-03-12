using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;

    public CategoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Add(Category category)
    {
        _appDbContext.Categories.Add(category);
    }

    public Category? GetById(int id)
    {
        return _appDbContext.Categories.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Category> GetAll()
    {
        return [.. _appDbContext.Categories];
    }

    public void SaveChanges()
    {
        _appDbContext.SaveChanges();
    }

    public void Delete(Category category)
    {
        _appDbContext.Categories.Remove(category);
    }
}