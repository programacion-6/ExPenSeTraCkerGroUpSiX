using RestApi.Domain;
using RestApi.Mappers.Interfaces;

namespace RestApi.Mappers.Concretes;

public class UpdateProfileRequest : CreateRequestTemplate<User>
{
    public required string Name { get; set; }
    public required string Email { get; set; }

    public User ToDomain()
    {
        return new User
        {
            Name = Name,
            Email = Email,
            Password = ""
        };
    }
}
