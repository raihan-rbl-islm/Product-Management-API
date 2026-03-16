using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Services;
using ProductManagementApi.Repositories;
using ProductManagementApi.DTOs;
using AutoMapper;

namespace ProductManagementApi.Models;

public class UserService : IUserService
{
    public readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<ReadUserDTO?> AuthenticateAsync(LoginRequestDto loginRequestDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginRequestDto.Username);
        return (user == null || user.Password != loginRequestDto.Password) ? null : _mapper.Map<ReadUserDTO>(user);
    }

    public async Task<ReadUserDTO?> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(registerRequestDto.Username);

        if (existingUser != null)
        {
            return null;
        }

        var newUser = _mapper.Map<User>(registerRequestDto);

        _userRepository.Add(newUser);
        await _userRepository.SaveChangesAsync();

        return _mapper.Map<ReadUserDTO>(newUser);
    }


}