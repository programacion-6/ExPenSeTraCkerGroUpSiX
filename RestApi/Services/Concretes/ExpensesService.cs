using System.Runtime.CompilerServices;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Persistence.DataBase;
using RestApi.Services.interfaces;
using System.Globalization;

namespace RestApi.Services.Concretes;

public class ExpenseService : IBaseService<Expense>
{
    private readonly ApplicationDbContext _context;

    public ExpenseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Expense> CreateAsync(Expense expense)
    {
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

    public async Task ValidateCategory(Expense expense, string NameCategory)
    {
        var category = await FindCategoryAsync(NameCategory);
        if (category == null)
        {
            throw new ArgumentException($"Category does not exist.");
        }

        expense.Category = category;
    }

    public async Task<ExpenseCategory> FindCategoryAsync(string NameCategory)
    {
        var category = await _context.ExpenseCategory.FirstOrDefaultAsync(c => c.Name == NameCategory);
        return category;
    }

    public async Task<string> GetMonthWithMostExpensesAsync()
    {
        var expensesGroupedByMonth = await _context.Expense
            .GroupBy(date => date.Date.Month)
            .Select(group => new
            {
                Month = group.Key,
                TotalExpenses = group.Sum(amount => amount.Amount)
            })
            .OrderByDescending(amount => amount.TotalExpenses)
            .FirstOrDefaultAsync();

        if (expensesGroupedByMonth == null)
        {
            return "Not there registered expenses.";
        }

        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(expensesGroupedByMonth.Month);
        return $"{monthName} is the month with more expense, total: {expensesGroupedByMonth.TotalExpenses}.";
    }

    public async Task<string> GetCategoryWithMostExpensesAsync()
    {
        var expensesGroupedByCategory = await _context.Expense
            .GroupBy(category => category.Category)
            .Select(group => new
            {
                Category = group.Key,
                TotalExpenses = group.Sum(amount => amount.Amount)
            })
            .OrderByDescending(amount => amount.TotalExpenses)
            .FirstOrDefaultAsync();

        if (expensesGroupedByCategory == null)
        {
            return "Not there registered expenses.";
        }

        var category = await _context.ExpenseCategory
            .FirstOrDefaultAsync(category => category.Id == expensesGroupedByCategory.Category.Id);

        return $"{category.Name} category with more expense, total: {expensesGroupedByCategory.TotalExpenses}.";
    }
}
