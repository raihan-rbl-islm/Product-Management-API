using ProductManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.DTOs;
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

    public (IEnumerable<Product>, int) QueryProducts(ProductQueryParameterDto queryParameters)
    {
        var query = _appDbContext.Products.AsNoTracking().AsQueryable();
        if (queryParameters.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= queryParameters.MinPrice.Value);
        }

        if (queryParameters.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= queryParameters.MaxPrice.Value);
        }

        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            var searchTerm = $"%{queryParameters.Search}%";
            query = query.Where(p =>
                EF.Functions.ILike(p.Name, searchTerm) ||
                EF.Functions.ILike(p.Description, searchTerm));
        }

        if (queryParameters.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == queryParameters.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            query = queryParameters.SortBy switch
            {
                "Name" => queryParameters.SortDescending == true
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),
                "Price" => queryParameters.SortDescending == true
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price),
                "LastUpdatedAt" => queryParameters.SortDescending == true
                    ? query.OrderByDescending(p => p.LastUpdatedAt)
                    : query.OrderBy(p => p.LastUpdatedAt),
                _ => query
            };
        }

        int totalCount = query.Count();

        var skip = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
        query = query.Skip(skip).Take(queryParameters.PageSize);

        return ([.. query], totalCount);
    }
}