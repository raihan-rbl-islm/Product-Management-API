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

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<ReadCategoryDto>> GetAllAsync()
    {
        return await _appDbContext.Categories
            .AsNoTracking()
            .ProjectTo<ReadCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public void Delete(Category category)
    {
        _appDbContext.Categories.Remove(category);
    }
}