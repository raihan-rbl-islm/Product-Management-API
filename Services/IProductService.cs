using ProductManagementApi.DTOs;
namespace ProductManagementApi.Services;

public interface IProductService
{
    int CreateProduct(CreateProductDto createProductDto);
    void UpdateProduct(int id, UpdateProductDto updateProductDto);
    ReadProductDto? ReadProduct(int id);
    List<ReadProductDto> ReadAllProducts();
}