using ProductManagementApi.Repositories;

namespace ProductManagementApi.Models;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public User? GetByUsername(string username)
    {
        return _appDbContext.Users.FirstOrDefault(u => u.Username == username);
    }

    public void Add(User user)
    {
        _appDbContext.Users.Add(user);
    }

    public void SaveChanges()
    {
        _appDbContext.SaveChanges();
    }

}