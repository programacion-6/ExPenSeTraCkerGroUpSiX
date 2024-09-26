namespace RestApi.Domain;
using FluentValidation;

public class ExpenseValidator: AbstractValidator<Expense>
{
    public ExpenseValidator()
    {
        RuleFor(expense => expense.Description).NotEmpty();
        RuleFor(expense => expense.Amount).NotEqual(0);
        RuleFor(expense => expense.Category).NotNull();
        RuleFor(expense => expense.Date).NotNull();
    }
}