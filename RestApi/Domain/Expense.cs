namespace RestApi.Domain;

public class Expense
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required decimal Amount { get; set; }
    public required DateTime Date { get; set; }
    public required string Description { get; set; }
    public required ExpenseCategory Category { get; set; }
}
