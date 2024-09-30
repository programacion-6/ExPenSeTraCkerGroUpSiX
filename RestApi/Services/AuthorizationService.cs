using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Persistence.DataBase;
using RestApi.JWT;

namespace RestApi.Services;
 public class AuthorizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenHandler _tokenHandler;

        public AuthorizationService(ApplicationDbContext context, TokenHandler tokenHandler)
        {
            _context = context;
            _tokenHandler = tokenHandler;
        }

        public async Task<string> RegisterUserAsync(User user)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return _tokenHandler.GenerateToken(user);
        }
        
        public async Task<string> LoginAsync(string email, string password)
        {
            
            // Busca el usuario por correo electrónico
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
