using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestApi.Domain;

namespace RestApi.JWT;

public class TokenHandler 
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtConstants.SecrectKey);
        var tokenDescriptor = CreateTokenDecriptor(user, key);
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor CreateTokenDecriptor(User user, byte[] key)
    {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name)
                }
            ),
            Expires = DateTime.UtcNow.AddDays(JwtConstants.ExpirationDays),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
    }
}
