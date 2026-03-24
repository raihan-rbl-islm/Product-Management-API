using System.ComponentModel.DataAnnotations;
using ProductManagementApi.Enums;
using System.Text.Json.Serialization;

namespace ProductManagementApi.DTOs;

public record RegisterRequestDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    public required string Username { get; init; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public required string Password { get; init; }
    [Required(ErrorMessage = "Role is required.")]
    [EnumDataType(typeof(RoleEnum), ErrorMessage = "Invalid role. Allowed values are Admin and User.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required RoleEnum Role { get; init; }
}