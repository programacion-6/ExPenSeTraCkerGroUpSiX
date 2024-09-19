namespace ExpenseTrackerGrup6.src.Domain;

public class UserItems
{
    public required Guid UserId { get; set; }
    public required List<Expense> Expenses { get; set; } = new();
    public required List<Income> Incomes { get; set; } = new();
    public required List<ExpenseCategory> ExpenseCategories { get; set; } = new();
}
