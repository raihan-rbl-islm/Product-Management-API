using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs;

public record UpdateCategoryDto
{
    [Required]
    [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
    public required string Name { get; init; }
}