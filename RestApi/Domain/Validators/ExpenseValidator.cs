namespace RestApi.Domain.Validators;
using FluentValidation;

public class ExpenseValidator: AbstractValidator<Expense>
{

    public ExpenseValidator() 
    {
        RuleFor(expense => expense.Description).NotEmpty().WithMessage("A category is required");
        RuleFor(expense => expense.Amount)
            .NotEmpty().WithMessage("A amount is required")
            .GreaterThanOrEqualTo(0).WithMessage("the amount has to be higher to 0");
    }
}
