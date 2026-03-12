using Microsoft.AspNetCore.Identity.Data;
using ProductManagementApi.Services;
using ProductManagementApi.Repositories;
using ProductManagementApi.DTOs;

namespace ProductManagementApi.Models;

public class UserService : IUserService
{
    public readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public ReadUserDTO? Authenticate(LoginRequestDto loginRequestDto)
    {

        var user = _userRepository.GetByUsername(loginRequestDto.Username);

        if (user == null || user.Password != loginRequestDto.Password)
        {
            return null;
        }

        return new ReadUserDTO
        {
            Username = user.Username,
            Role = user.Role
        };
    }

    public ReadUserDTO? Register(RegisterRequestDto registerRequestDto)
    {
        var existingUser = _userRepository.GetByUsername(registerRequestDto.Username);
        if (existingUser != null)
        {
            return null;
        }

        var newUser = new User
        {
            Username = registerRequestDto.Username,
            Password = registerRequestDto.Password,
            Role = registerRequestDto.Role
        };

        _userRepository.Add(newUser);
        _userRepository.SaveChanges();

        return new ReadUserDTO
        {
            Username = newUser.Username,
            Role = newUser.Role
        };
    }


}