using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;
namespace ProductManagementApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public ReadProductDto CreateProduct(CreateProductDto createProductDto)
    {
        var newProduct = new Product
        {
            Name = createProductDto.Name,
            Price = createProductDto.Price,
            Description = createProductDto.Description,
            // We ignore Category mapping here for simplicity unless we have a CategoryId
            Stock = createProductDto.Stock ?? 0,
            LastUpdatedAt = DateTime.UtcNow
        };

        _productRepository.Add(newProduct);
        _productRepository.SaveChanges();

        return new ReadProductDto
        {
            Id = newProduct.Id,
            Name = newProduct.Name,
            Price = newProduct.Price,
            Description = newProduct.Description,
            CategoryName = null, // Not set during creation for now
            Stock = newProduct.Stock
        };
    }

    public bool UpdateProduct(int id, UpdateProductDto updateProductDto)
    {
        // Tracking: GetById fetches the entity which is then tracked by EF
        var product = _productRepository.GetById(id);

        if (product == null) return false;

        product.Price = updateProductDto.Price;
        product.Description = updateProductDto.Description;
        // Skipping Category update for now as it's a separate entity
        product.Stock = updateProductDto.Stock;

        _productRepository.Update(product);
        _productRepository.SaveChanges();

        return true;
    }

    public ReadProductDto? ReadProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null) return null;

        // Lazy Loading: Accessing product.Category here will trigger a database query
        // because we enabled LazyLoadingProxies and Category is virtual.
        var categoryName = product.Category?.Name;

        return new ReadProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            CategoryName = categoryName,
            Stock = product.Stock
        };
    }

    public bool DeleteProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null) return false;

        _productRepository.Delete(product);
        _productRepository.SaveChanges();

        return true;
    }

    public IEnumerable<ReadProductDto> GetByPriceRange(decimal minPrice, decimal maxPrice)
    {
        var products = _productRepository.GetByPriceRange(minPrice, maxPrice);

        return [.. products.Select(p =>
            new ReadProductDto {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryName = p.Category?.Name,
                Stock = p.Stock
            })
        ];
    }
}