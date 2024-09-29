using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Persistence.DataBase;
using RestApi.Services.interfaces;

namespace RestApi.Services.Concretes;

public class ExpenseService : IBaseService<Expense>
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<Expense> _validator;

    public ExpenseService(ApplicationDbContext context, IValidator<Expense> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Expense> CreateAsync(Expense expense)
    {
        var validationResult = await _validator.ValidateAsync(expense);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        _context.Expense.Add(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<List<Expense>> ReadAsync()
    {
        return await _context.Expense
            .Include(e => e.Category)
            .Where(e => e.UserId == UserContext.CurrentUserId)
            .ToListAsync();
    }

    public async Task<Expense?> ReadAsync(Guid id)
    {
        return await _context.Expense
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> UpdateAsync(Guid id, Expense updatedExpense)
    {
        var existingExpense = await _context.Expense.FindAsync(id);
        if (existingExpense == null) return false;

        existingExpense.Amount = updatedExpense.Amount;
        existingExpense.Date = updatedExpense.Date;
        existingExpense.Description = updatedExpense.Description;
        existingExpense.Category = updatedExpense.Category;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var expense = await _context.Expense.FindAsync(id);
        if (expense == null) return false;

        _context.Expense.Remove(expense);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Expense>> GetExpensesAsync(DateTime? date, string category)
    {
        if (!date.HasValue && string.IsNullOrEmpty(category)) 
        {
            return await ReadAsync();
        }

        var query = _context.Expense
            .Where(e => e.UserId == UserContext.CurrentUserId)
            .AsQueryable();

        if (date.HasValue)
            query = query.Where(e => e.Date.Month == date.Value.Month);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.Category.Name == category);

        return await query.Include(e => e.Category).ToListAsync();
    }
}
