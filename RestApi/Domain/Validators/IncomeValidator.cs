namespace RestApi.Domain.Validators;
using FluentValidation;

public class IncomeValidator: AbstractValidator<Income>
{
    public IncomeValidator()
    {
        RuleFor(Income => Income.Amount).NotEmpty().WithMessage("A amount is required").LessThanOrEqualTo(0).WithMessage("the amount has to be higher to 0");
        RuleFor(Income => Income.Source).NotEmpty().WithMessage("A source is required");
        RuleFor(Income => Income.Date).NotEmpty().WithMessage("A Date is required");
    }
}