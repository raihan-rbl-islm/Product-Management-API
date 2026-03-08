using System.ComponentModel.DataAnnotations;
namespace ProductManagementApi.DTOs;

public record ReadProductDto
{
    public int Id { get; init; }
    public required string Name { get; init; }

    public required decimal Price { get; init; }

    public string? Description { get; init; }

    public string? Category { get; init; }

    public int Stock { get; init; }
}