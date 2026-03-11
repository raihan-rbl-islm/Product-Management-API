namespace ProductManagementApi.Services;

public interface ITokenService
{
    string GenerateJwtToken(string username);
}
