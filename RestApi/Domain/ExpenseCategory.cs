namespace RestApi.Domain;

public class ExpenseCategory
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
}
