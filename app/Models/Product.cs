using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementApi.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int Stock { get; set; }

    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    public DateTime LastUpdatedAt { get; set; }
}