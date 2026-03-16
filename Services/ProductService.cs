using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;
using AutoMapper;
namespace ProductManagementApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<ReadProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        if (createProductDto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(createProductDto.CategoryId.Value);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {createProductDto.CategoryId} not found.");
            }
        }

        var newProduct = _mapper.Map<Product>(createProductDto);
        newProduct.LastUpdatedAt = DateTime.UtcNow;

        _productRepository.Add(newProduct);
        await _productRepository.SaveChangesAsync();

        return _mapper.Map<ReadProductDto>(newProduct);
    }

    public async Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null) return false;

        if (updateProductDto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(updateProductDto.CategoryId.Value);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {updateProductDto.CategoryId} not found.");
            }
        }

        _mapper.Map(updateProductDto, product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<ReadProductDto?> ReadProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        return (product == null) ? null : _mapper.Map<ReadProductDto>(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null) return false;

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResponse<ReadProductDto>> QueryProductsAsync(ProductQueryParameterDto queryParameters)
    {
        // var (products, totalCount) = _productRepository.QueryProducts(queryParameters);
        // var productDtos = _mapper.Map<IEnumerable<ReadProductDto>>(products);
        var (productDTOs, totalCount) = await _productRepository.QueryProductsAsync(queryParameters);
        return new PagedResponse<ReadProductDto>
        {
            Items = productDTOs,
            TotalCount = totalCount,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize
        };
    }
}