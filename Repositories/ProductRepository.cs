using ProductManagementApi.Models;
using Microsoft.EntityFrameworkCore;
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
        // Tracking is default here
        return _appDbContext.Products.FirstOrDefault(p => p.Id == id);
    }

    public void Update(Product product)
    {
        product.LastUpdatedAt = DateTime.UtcNow;
        // Tracking: EF already tracks this if it was fetched with GetById
        _appDbContext.Products.Update(product);
    }

    public void Delete(Product product)
    {
        _appDbContext.Products.Remove(product);
    }

    public void SaveChanges()
    {
        _appDbContext.SaveChanges();
    }

    public IEnumerable<Product> GetByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return [.. _appDbContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p =>
                p.Price >= minPrice &&
                p.Price <= maxPrice)
        ];
    }
}