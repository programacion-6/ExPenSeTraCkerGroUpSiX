using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Persistence.DataBase;
using RestApi.Services.interfaces;

namespace RestApi.Services.Concretes;

public class IncomeService : IBaseService<Income>
{
    private readonly ApplicationDbContext _context;

    public IncomeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Income> CreateAsync(Income income)
    {
        _context.Income.Add(income);
        await _context.SaveChangesAsync();
        return income;
    }

    public async Task<List<Income>> ReadAsync()
    {
        return await _context.Income
            .Where(e => e.UserId == UserContext.CurrentUserId)
            .ToListAsync();
    }

    public async Task<Income?> ReadAsync(Guid id)
    {
        return await _context.Income.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> UpdateAsync(Guid id, Income income)
    {
        var existingIncome = await _context.Income.FindAsync(id);
        if (existingIncome == null) return false;

        existingIncome.Amount = income.Amount;
        existingIncome.Date = income.Date;
        existingIncome.Source = income.Source;

        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var income = await _context.Income.FindAsync(id);
        if (income == null) return false;

        _context.Income.Remove(income);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Income>> GetIncomesAsync(DateTime? date)
    {
        if (!date.HasValue) 
        {
            return await ReadAsync();
        }   

        return await _context.Income
            .Where(e => e.UserId == UserContext.CurrentUserId && e.Date.Month == date.Value.Month)
            .ToListAsync();
    }
}
