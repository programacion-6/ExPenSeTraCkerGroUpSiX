using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Persistence.DataBase;

namespace RestApi.Services
{
    public class ExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Expense> CreateAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<List<Expense>> ReadAsync()
        {
            return await _context.Expenses
                .Include(e => e.Category) 
                .ToListAsync();
        }

        public async Task<Expense?> ReadAsync(Guid id)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateAsync(Guid id, Expense updatedExpense)
        {
            var existingExpense = await _context.Expenses.FindAsync(id);
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
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
