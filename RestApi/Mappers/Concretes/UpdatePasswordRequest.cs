using RestApi.Domain;
using RestApi.Mappers.Interfaces;

namespace RestApi.Mappers.Concretes;

public class UpdatePasswordRequest : CreateRequestTemplate<User.PasswordUpdate>
{

    public required string Code  { get; set; }
    public required string Password { get; set; }

    public User.PasswordUpdate ToDomain()
    {
        return new User.PasswordUpdate
        {
            Code = Code,
            Password = Password,
        };
    }
}