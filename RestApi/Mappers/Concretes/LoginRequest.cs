using RestApi.Domain;
using RestApi.Mappers.Interfaces;
using RestApi.Persistence.DataBase;

namespace RestApi.Mappers.Concretes;
public record LoginRequest  : CreateRequestTemplate<User>
{
    public required string Email { get; set; }
    public required string Password { get; set; }

    public User ToDomain()
    {
        return new User
        {
            Email = Email,
            Password = Password 
        };
    }
}
