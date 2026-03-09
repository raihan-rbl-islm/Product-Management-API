using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;
namespace ProductManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var newProduct = _productService.CreateProduct(createProductDto);
        _logger.LogInformation("Product {ProductName} created successfully at {Timestamp}", newProduct.Name, DateTime.UtcNow);
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
    public IActionResult ReadAllProducts([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
    {
        var productDtos = _productService.GetByPriceRange(minPrice ?? 0, maxPrice ?? decimal.MaxValue);
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