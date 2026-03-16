using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
namespace ProductManagementApi.Services;

public interface IProductService
{
    Task<ReadProductDto> CreateProductAsync(CreateProductDto createProductDto);
    Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    Task<ReadProductDto?> ReadProductAsync(int id);
    Task<bool> DeleteProductAsync(int id);
    Task<PagedResponse<ReadProductDto>> QueryProductsAsync(ProductQueryParameterDto queryParameters);
}