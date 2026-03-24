using ProductManagementApi.Models;
using ProductManagementApi.DTOs;
namespace ProductManagementApi.Repositories;

public interface IProductRepository
{
    void Add(Product product);
    Task<Product?> GetByIdAsync(int id);
    void Update(Product product);
    Task SaveChangesAsync();
    void Delete(Product product);
    Task<(IEnumerable<ReadProductDto>, int)> QueryProductsAsync(ProductQueryParameterDto queryParameters);
}