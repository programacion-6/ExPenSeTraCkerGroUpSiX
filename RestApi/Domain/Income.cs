namespace RestApi.Domain;
public class Income
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid UserId { get; init; } 
    public required decimal Amount { get; set; }
    public required DateTime Date { get; set; }
    public required string Source { get; set; }
}