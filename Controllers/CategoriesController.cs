using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagementApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        var newCategory = _categoryService.CreateCategoryAsync(createCategoryDto);
        return Ok(newCategory);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
    {
        var success = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);

        if (!success) return NotFound();

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

        if (categoryDto == null) return NotFound();

        return Ok(categoryDto);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var success = await _categoryService.DeleteCategoryAsync(id);

        if (!success) return NotFound();

        return NoContent();
    }
}