using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories;

public interface IUserRepository
{
    User? GetByUsername(string username);
    void Add(User user);
    void SaveChanges();
}