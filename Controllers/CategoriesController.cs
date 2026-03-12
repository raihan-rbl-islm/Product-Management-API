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
    public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        var newCategory = _categoryService.CreateCategory(createCategoryDto);
        return Ok(newCategory);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPut("{id:int}")]
    public IActionResult UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
    {
        var success = _categoryService.UpdateCategory(id, updateCategoryDto);

        if (!success) return NotFound();

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public IActionResult GetCategory(int id)
    {
        var categoryDto = _categoryService.GetCategoryById(id);

        if (categoryDto == null) return NotFound();

        return Ok(categoryDto);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetAllCategories()
    {
        var categories = _categoryService.GetAllCategories();
        return Ok(categories);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public IActionResult DeleteCategory(int id)
    {
        var success = _categoryService.DeleteCategory(id);

        if (!success) return NotFound();

        return NoContent();
    }
}