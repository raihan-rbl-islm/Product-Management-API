using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;
namespace ProductManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var newProduct = _productService.CreateProduct(createProductDto);
        return Ok(newProduct);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        var success = _productService.UpdateProduct(id, updateProductDto);

        if (!success) return NotFound();

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public IActionResult ReadProduct(int id)
    {
        var productDto = _productService.ReadProduct(id);

        if (productDto == null) return NotFound();

        return Ok(productDto);
    }

    [HttpGet]
    public IActionResult ReadAllProducts()
    {
        var productDtos = _productService.ReadAllProducts();

        return Ok(productDtos);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteProduct(int id)
    {
        var success = _productService.DeleteProduct(id);

        if (!success) return NotFound();

        return NoContent();
    }
}