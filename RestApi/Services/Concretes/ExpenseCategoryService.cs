using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Domain.Validators;
using RestApi.Persistence.DataBase;
using FluentValidation;

namespace RestApi.Services.Concretes
{
    public class ExpenseCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<ExpenseCategory> _validator;

        public ExpenseCategoryService(ApplicationDbContext context, IValidator<ExpenseCategory> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ExpenseCategory> CreateAsync(ExpenseCategory category)
        {
            var validationResult = await _validator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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
            return await _context.ExpenseCategory.FirstOrDefaultAsync(category => category.Id == id);
        }

        public async Task<bool> UpdateAsync(Guid id, ExpenseCategory updatedCategory)
        {
            var validationResult = await _validator.ValidateAsync(updatedCategory);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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