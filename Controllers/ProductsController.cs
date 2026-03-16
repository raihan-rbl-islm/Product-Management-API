using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagementApi.Controllers;

[Authorize]
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

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var newProduct = await _productService.CreateProductAsync(createProductDto);
        _logger.LogInformation("Product {ProductName} created successfully at {Timestamp}", newProduct.Name, DateTime.UtcNow);
        return Ok(newProduct);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        var success = await _productService.UpdateProductAsync(id, updateProductDto);
        if (!success) return NotFound();

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ReadProduct(int id)
    {
        var productDto = await _productService.ReadProductAsync(id);

        if (productDto == null) return NotFound();

        return Ok(productDto);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> QueryProducts([FromQuery] ProductQueryParameterDto queryParameters)
    {
        return Ok(await _productService.QueryProductsAsync(queryParameters));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var success = await _productService.DeleteProductAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}