using ProductManagementApi.Enums;
namespace ProductManagementApi.Models;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public string Username { get; set; } = String.Empty;

    public string Password { get; set; } = String.Empty;

    public RoleEnum Role { get; set; } = RoleEnum.Admin;
}

