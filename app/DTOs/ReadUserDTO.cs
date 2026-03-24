using ProductManagementApi.Enums;
using System.Text.Json.Serialization;

namespace ProductManagementApi.DTOs;

public record ReadUserDTO
{
    public required string Username { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required RoleEnum Role { get; init; }
}