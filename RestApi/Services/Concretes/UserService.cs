using RestApi.Persistence.DataBase;
using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Services.interfaces;

namespace RestApi.Services.Concretes;

public class UserService : IBaseService<User>
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<List<User>> ReadAsync()
    {
        return await _context.User.ToListAsync();           
    }
    public async Task<User> ReadAsync(Guid id)
    {
        var user = await _context.User.FirstOrDefaultAsync(e => e.Id == id);
        if (user == null) {

        }

        return user;
    }

    public async Task<bool> UpdateAsync(Guid id, User user)
    {
        var existingUser = await _context.User.FindAsync(id);
        if (existingUser == null) return false;

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
         var user = await _context.User.FindAsync(id);
        if (user == null) return false;

        _context.User.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
