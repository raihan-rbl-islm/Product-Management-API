using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
namespace ProductManagementApi.Services;

public interface IUserService
{
    Task<ReadUserDTO?> AuthenticateAsync(LoginRequestDto loginRequestDto);
    Task<ReadUserDTO?> RegisterAsync(RegisterRequestDto registerRequestDto);
}