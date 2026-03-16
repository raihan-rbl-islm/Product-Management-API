using ProductManagementApi.Models;
using ProductManagementApi.DTOs;
namespace ProductManagementApi.Repositories;

public interface IProductRepository
{
    void Add(Product product);
    Product? GetById(int id);
    void Update(Product product);
    void SaveChanges();
    void Delete(Product product);
    (IEnumerable<ReadProductDto>, int) QueryProducts(ProductQueryParameterDto queryParameters);
}