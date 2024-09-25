using RestApi.Domain;

namespace RestApi.Mappers;

public class CreateExpenseRequest
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public required string Description { get; set; }
    public required ExpenseCategory Category { get; set; }

    public Expense ToDomain()
    {
        return new Expense
        {
            Amount = Amount,
            Date = Date,
            Description = Description,
            Category = Category
        };
    }
}
