using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int Stock { get; set; }

    public string? Category { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }
}