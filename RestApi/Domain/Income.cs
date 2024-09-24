namespace RestApi.Domain;

public class Income
{
    public required Guid Id { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime Date { get; set; }
    public required string Source { get; set; }
}
