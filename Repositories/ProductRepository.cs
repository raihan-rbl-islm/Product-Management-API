using ProductManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.DTOs;
using AutoMapper.QueryableExtensions;
using AutoMapper;
namespace ProductManagementApi.Repositories;

public class ProductRepository : IProductRepository
{

    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public ProductRepository(AppDbContext appDbContext, IMapper mapper)
    {
        this._appDbContext = appDbContext;
        this._mapper = mapper;
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

    public (IEnumerable<ReadProductDto>, int) QueryProducts(ProductQueryParameterDto queryParameters)
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
                (p.Description != null && EF.Functions.ILike(p.Description, searchTerm)));
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
        var productDtos = query
                .Skip(skip)
                .Take(queryParameters.PageSize)
                .ProjectTo<ReadProductDto>(_mapper.ConfigurationProvider);

        return (productDtos, totalCount);
    }
}