using ProductManagementApi.Models;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using ProductManagementApi.DTOs;
using Microsoft.EntityFrameworkCore;
namespace ProductManagementApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CategoryRepository(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public void Add(Category category)
    {
        _appDbContext.Categories.Add(category);
    }

    public Category? GetById(int id)
    {
        return _appDbContext.Categories.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<ReadCategoryDto> GetAll()
    {
        return _appDbContext.Categories
            .AsNoTracking()
            .ProjectTo<ReadCategoryDto>(_mapper.ConfigurationProvider);
    }

    public void SaveChanges()
    {
        _appDbContext.SaveChanges();
    }

    public void Delete(Category category)
    {
        _appDbContext.Categories.Remove(category);
    }
}