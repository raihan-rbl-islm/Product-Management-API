namespace ProductManagementApi.DTOs;

public record ReadCategoryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}