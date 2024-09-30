namespace RestApi.Domain;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
