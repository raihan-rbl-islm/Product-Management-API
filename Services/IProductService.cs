using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
namespace ProductManagementApi.Services;

public interface IProductService
{
    ReadProductDto CreateProduct(CreateProductDto createProductDto);
    bool UpdateProduct(int id, UpdateProductDto updateProductDto);
    ReadProductDto? ReadProduct(int id);
    List<ReadProductDto> ReadAllProducts();
}