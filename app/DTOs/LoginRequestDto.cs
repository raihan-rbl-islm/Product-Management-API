using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs;

public record LoginRequestDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    public required string Username { get; init; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    [DataType(DataType.Password)]
    public required string Password { get; init; }
}