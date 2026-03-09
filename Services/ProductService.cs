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
            Category = createProductDto.Category,
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
            Category = newProduct.Category,
            Stock = newProduct.Stock
        };
    }

    public bool UpdateProduct(int id, UpdateProductDto updateProductDto)
    {
        var product = _productRepository.GetById(id);

        if (product == null) return false;

        product.Price = updateProductDto.Price;
        product.Description = updateProductDto.Description;
        product.Category = updateProductDto.Category;
        product.Stock = updateProductDto.Stock;

        _productRepository.Update(product);
        _productRepository.SaveChanges();

        return true;
    }

    public ReadProductDto? ReadProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null) return null;

        return new ReadProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Category = product.Category,
            Stock = product.Stock
        };
    }

    public List<ReadProductDto> ReadAllProducts()
    {
        var products = _productRepository.GetAll();

        List<ReadProductDto> readProductDtos = [];

        foreach (var item in products)
        {
            readProductDtos.Add(
             new ReadProductDto
             {
                 Id = item.Id,
                 Name = item.Name,
                 Price = item.Price,
                 Description = item.Description,
                 Category = item.Category,
                 Stock = item.Stock
             });
        }

        return readProductDtos;
    }

    public bool DeleteProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null) return false;

        _productRepository.Delete(product);
        _productRepository.SaveChanges();

        return true;
    }
}