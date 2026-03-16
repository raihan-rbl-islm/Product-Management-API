using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;
using AutoMapper;
namespace ProductManagementApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public ReadCategoryDto CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var newCategory = _mapper.Map<Category>(createCategoryDto);

        _categoryRepository.Add(newCategory);
        _categoryRepository.SaveChanges();

        return _mapper.Map<ReadCategoryDto>(newCategory);
    }

    public ReadCategoryDto? GetCategoryById(int id)
    {
        var category = _categoryRepository.GetById(id);
        return (category == null) ? null : _mapper.Map<ReadCategoryDto>(category);
    }

    public IEnumerable<ReadCategoryDto> GetAllCategories()
    {
        return _categoryRepository.GetAll();
    }

    public bool UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
    {
        var category = _categoryRepository.GetById(id);

        if (category == null) return false;

        _mapper.Map(updateCategoryDto, category);
        _categoryRepository.SaveChanges();

        return true;
    }

    public bool DeleteCategory(int id)
    {
        var category = _categoryRepository.GetById(id);

        if (category == null) return false;

        _categoryRepository.Delete(category);
        _categoryRepository.SaveChanges();

        return true;
    }
}