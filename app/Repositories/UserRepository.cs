using ProductManagementApi.Repositories;
using Microsoft.EntityFrameworkCore;
namespace ProductManagementApi.Models;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public void Add(User user)
    {
        _appDbContext.Users.Add(user);
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

}