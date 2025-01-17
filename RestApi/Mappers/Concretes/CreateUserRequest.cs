using RestApi.Domain;
using RestApi.Mappers.Interfaces;

namespace RestApi.Mappers.Concretes;

public class CreateUserRequest : CreateRequestTemplate<User>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    public User ToDomain()
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = this.Name,
            Email = this.Email,
            Password = this.Password 
        };
    }
}
