using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;
namespace ProductManagementApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public ReadProductDto CreateProduct(CreateProductDto createProductDto)
    {
        if (createProductDto.CategoryId.HasValue)
        {
            var category = _categoryRepository.GetById(createProductDto.CategoryId.Value);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {createProductDto.CategoryId} not found.");
            }
        }

        var newProduct = new Product
        {
            Name = createProductDto.Name,
            Price = createProductDto.Price,
            Description = createProductDto.Description,
            CategoryId = createProductDto.CategoryId,
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
            CategoryName = newProduct.Category?.Name,
            Stock = newProduct.Stock
        };
    }

    public bool UpdateProduct(int id, UpdateProductDto updateProductDto)
    {
        var product = _productRepository.GetById(id);

        if (product == null) return false;

        if (updateProductDto.CategoryId.HasValue)
        {
            var category = _categoryRepository.GetById(updateProductDto.CategoryId.Value);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {updateProductDto.CategoryId} not found.");
            }
        }

        product.Price = updateProductDto.Price;
        product.Description = updateProductDto.Description;
        product.CategoryId = updateProductDto.CategoryId;
        product.Stock = updateProductDto.Stock;

        _productRepository.Update(product);
        _productRepository.SaveChanges();

        return true;
    }

    public ReadProductDto? ReadProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null) return null;

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