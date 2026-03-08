using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];
    private int _nextId = 1;

    public void Add(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
    }

    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public void Update(Product product)
    {
        var existingProduct = GetById(product.Id);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.Stock = product.Stock;
            existingProduct.Category = product.Category;
            existingProduct.LastUpdatedAt = DateTime.UtcNow;
        }
    }


    public List<Product> GetAll()
    {
        return _products;
    }

    public void SaveChanges() { }
}