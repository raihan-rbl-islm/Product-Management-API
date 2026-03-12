using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
namespace ProductManagementApi.Services;

public interface IUserService
{
    ReadUserDTO? Authenticate(LoginRequestDto loginRequestDto);
    ReadUserDTO? Register(RegisterRequestDto registerRequestDto);
}