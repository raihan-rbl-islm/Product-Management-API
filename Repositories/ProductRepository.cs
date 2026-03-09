using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public class ProductRepository : IProductRepository
{

    private readonly AppDbContext _appDbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        this._appDbContext = appDbContext;
    }

    public void Add(Product product)
    {
        _appDbContext.Products.Add(product);
    }

    public Product? GetById(int id)
    {
        return _appDbContext.Products.FirstOrDefault(p => p.Id == id);
    }

    public void Update(Product product)
    {
        product.LastUpdatedAt = DateTime.UtcNow;
        _appDbContext.Products.Update(product);
    }

    public IEnumerable<Product> GetAll()
    {
        return _appDbContext.Products.ToList();
    }
    
    public void Delete(Product product)
    {
        _appDbContext.Products.Remove(product);
    }

    public void SaveChanges()
    {
        _appDbContext.SaveChanges();
    }
}