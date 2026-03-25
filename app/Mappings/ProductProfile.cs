using ProductManagementApi.DTOs;
using ProductManagementApi.Models;

namespace ProductManagementApi.Mappings;

public class ProductProfile : AutoMapper.Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ReadProductDto>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Product, ReadProductDto>();
        CreateMap<Category, ReadCategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<User, ReadUserDTO>();
        CreateMap<RegisterRequestDto, User>();
    }
}