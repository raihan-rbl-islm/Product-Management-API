using System.ComponentModel.DataAnnotations;
namespace ProductManagementApi.DTOs;

public record CreateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 10000.00, ErrorMessage = "Price must be strictly positive.")]
    public required decimal Price { get; init; }

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive integer.")]
    public int? CategoryId { get; init; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int? Stock { get; init; }
}