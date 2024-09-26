using FluentValidation.Results;
using RestApi.Domain;

namespace RestApi.Tests.Domain;

public class ExpenseTest
{
   [Fact]
   public void ValidateExpense()
   {
      Expense expense = new()
      {
         Id = new Guid(),
         Amount = 1,
         Date = new DateTime(2000, 1, 1),
         Description = "This is a valid expense",
         Category = new ExpenseCategory
         {
            Id = new Guid(),
            Name = "Vehicle"
         }
      };

      ExpenseValidator expenseValidator = new();
      ValidationResult result = expenseValidator.Validate(expense);
      Assert.True(result.IsValid);
   }
}