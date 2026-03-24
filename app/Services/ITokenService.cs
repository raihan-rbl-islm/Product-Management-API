using ProductManagementApi.DTOs;

namespace ProductManagementApi.Services;

public interface ITokenService
{
    string GenerateJwtToken(ReadUserDTO readUserDTO);
}
