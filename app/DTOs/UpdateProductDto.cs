using System.ComponentModel.DataAnnotations;
namespace ProductManagementApi.DTOs;

public record UpdateProductDto
{
    [Range(0.01, 10000.00, ErrorMessage = "Price must be strictly positive.")]
    public decimal Price { get; init; }

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive integer.")]
    public int? CategoryId { get; init; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; init; }
}