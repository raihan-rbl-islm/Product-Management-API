using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public interface IProductRepository
{
    void Add(Product product);
    Product? GetById(int id);
    void Update(Product product);
    void SaveChanges();
    IEnumerable<Product> GetAll();
}