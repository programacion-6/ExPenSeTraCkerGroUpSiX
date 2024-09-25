using RestApi.Persistence.DataBase;
using Microsoft.EntityFrameworkCore;
using RestApi.Domain;

namespace RestApi.Services;

public class UserManager
{
    private readonly ApplicationDbContext _context;

    public UserManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<List<User>> ReadAsync(Guid? id = null)
    {
        var query = _context.Users.AsQueryable();

        if (id.HasValue)
            query = query.Where(u => u.Id == id.Value);

        return await query.ToListAsync();
    }

    public async Task<bool> UpdateAsync(Guid id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null) return false;

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password; // Asegúrate de encriptar la contraseña

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
         var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
