using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;
using Microsoft.EntityFrameworkCore;
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

    public async Task<ReadCategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        var newCategory = _mapper.Map<Category>(createCategoryDto);

        _categoryRepository.Add(newCategory);
        await _categoryRepository.SaveChangesAsync();

        return _mapper.Map<ReadCategoryDto>(newCategory);
    }

    public async Task<ReadCategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return (category == null) ? null : _mapper.Map<ReadCategoryDto>(category);
    }

    public async Task<IEnumerable<ReadCategoryDto>> GetAllCategoriesAsync()
    {
        return (await _categoryRepository.GetAllAsync()).Select(c => _mapper.Map<ReadCategoryDto>(c));
    }

    public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null) return false;

        _mapper.Map(updateCategoryDto, category);
        await _categoryRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null) return false;

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();

        return true;
    }
}