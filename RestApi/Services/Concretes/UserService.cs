using RestApi.Persistence.DataBase;
using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Services.interfaces;
using FluentValidation;
using RestApi.JWT;
namespace RestApi.Services.Concretes;

public class UserService : IBaseService<User>
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<User> _validator;
    private readonly TokenHandler _tokenHandler;

    public UserService(ApplicationDbContext context, IValidator<User> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<User> CreateAsync(User user)
    {
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }   
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
    public async Task<string> Login(string email, string password)
    {
        var userFound = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        UserContext.CurrentUserId = userFound.Id; // Establece el ID del usuario actual.

        if (userFound != null && userFound.Password == password) // Compara directamente las contraseñas
        {
            // Si la contraseña coincide, genera y devuelve el token
            return _tokenHandler.GenerateToken(userFound);
        }

        throw new AuthenticationException("Invalid email or password");
    }
}
