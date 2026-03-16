using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    void Add(User user);
    Task SaveChangesAsync();
}