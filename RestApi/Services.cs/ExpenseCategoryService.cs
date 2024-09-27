using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Persistence.DataBase;

namespace RestApi.Services
{
    public class ExpenseCategoryService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExpenseCategory> CreateAsync(ExpenseCategory category)
        {
            _context.ExpenseCategory.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<ExpenseCategory>> ReadAsync()
        {
            return await _context.ExpenseCategory.ToListAsync();
        }

        public async Task<ExpenseCategory?> ReadAsync(Guid id)
        {
            return await _context.ExpenseCategory.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateAsync(Guid id, ExpenseCategory updatedCategory)
        {
            var existingCategory = await _context.ExpenseCategory.FindAsync(id);
            if (existingCategory == null) return false;

            existingCategory.Name = updatedCategory.Name;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _context.ExpenseCategory.FindAsync(id);
            if (category == null) return false;

            _context.ExpenseCategory.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}