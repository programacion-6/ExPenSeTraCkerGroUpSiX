using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Persistence.DataBase;
namespace RestApi.Services;

public class IncomeService
{
    private readonly ApplicationDbContext _context;

    public IncomeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Income> CreateAsync(Income income)
    {
        _context.Incomes.Add(income);
        await _context.SaveChangesAsync();
        return income;
    }

    public async Task<List<Income>> ReadAsync(DateTime? startDate = null, DateTime? endDate = null, string category = null)
    {
        var query = _context.Incomes.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(i => i.Date >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(i => i.Date <= endDate.Value);
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(i => i.Source == category);

        return await query.ToListAsync();
    }


    public async Task<bool> UpdateAsync(Guid id, Income income)
    {
        var existingIncome = await _context.Incomes.FindAsync(id);
        if (existingIncome == null) return false;

        existingIncome.Amount = income.Amount;
        existingIncome.Date = income.Date;
        existingIncome.Source = income.Source;

        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var income = await _context.Incomes.FindAsync(id);
        if (income == null) return false;

        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync();
        return true;
    }


}
