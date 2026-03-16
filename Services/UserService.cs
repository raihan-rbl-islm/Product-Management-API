using Microsoft.AspNetCore.Identity.Data;
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
    public ReadUserDTO? Authenticate(LoginRequestDto loginRequestDto)
    {
        var user = _userRepository.GetByUsername(loginRequestDto.Username);
        return (user == null || user.Password != loginRequestDto.Password) ? null : _mapper.Map<ReadUserDTO>(user);
    }

    public ReadUserDTO? Register(RegisterRequestDto registerRequestDto)
    {
        var existingUser = _userRepository.GetByUsername(registerRequestDto.Username);

        if (existingUser != null)
        {
            return null;
        }

        var newUser = _mapper.Map<User>(registerRequestDto);

        _userRepository.Add(newUser);
        _userRepository.SaveChanges();

        return _mapper.Map<ReadUserDTO>(newUser);
    }


}