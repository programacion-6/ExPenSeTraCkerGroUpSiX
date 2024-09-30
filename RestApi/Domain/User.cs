namespace RestApi.Domain;

public class User
{
    public class PasswordUpdate
    {
        public required int Code { get; set; }
        public required string Password { get; set; }
    }
    
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
