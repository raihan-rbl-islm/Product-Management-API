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

        var newProduct = _mapper.Map<Product>(createProductDto);
        newProduct.LastUpdatedAt = DateTime.UtcNow;

        _productRepository.Add(newProduct);
        _productRepository.SaveChanges();

        return _mapper.Map<ReadProductDto>(newProduct);
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

        _mapper.Map(updateProductDto, product);
        _productRepository.SaveChanges();

        return true;
    }

    public ReadProductDto? ReadProduct(int id)
    {
        var product = _productRepository.GetById(id);

        return (product == null) ? null : _mapper.Map<ReadProductDto>(product);
    }

    public bool DeleteProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null) return false;

        _productRepository.Delete(product);
        _productRepository.SaveChanges();

        return true;
    }

    public PagedResponse<ReadProductDto> QueryProducts(ProductQueryParameterDto queryParameters)
    {
        // var (products, totalCount) = _productRepository.QueryProducts(queryParameters);
        // var productDtos = _mapper.Map<IEnumerable<ReadProductDto>>(products);
        var (productDTOs, totalCount) = _productRepository.QueryProducts(queryParameters);
        return new PagedResponse<ReadProductDto>
        {
            Items = productDTOs,
            TotalCount = totalCount,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize
        };
    }
}