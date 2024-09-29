using FluentValidation;
using RestApi.Persistence.DataBase;
using RestApi.Domain;
using Microsoft.EntityFrameworkCore;
using RestApi.Persistence.DataBase;

namespace RestApi.Domain.Validators;

public class ExpenseCategoryValidator : AbstractValidator<ExpenseCategory>
{
    private readonly ApplicationDbContext _context;

    public ExpenseCategoryValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("The name is required.")
            .MustAsync(BeUniqueName).WithMessage("The name already exists.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _context.ExpenseCategory
            .AnyAsync(c => c.Name == name, cancellationToken);
    }
}