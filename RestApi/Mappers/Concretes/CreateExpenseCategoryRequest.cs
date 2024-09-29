using RestApi.Domain;
using RestApi.Mappers.Interfaces;

namespace RestApi.Mappers.Concretes
{
    public class CreateExpenseCategoryRequest : CreateRequestTemplate<ExpenseCategory>
    {
        public required string Name { get; set; }

        public ExpenseCategory ToDomain()
        {
            return new ExpenseCategory
            {
                Name = Name
            };
        }
    }
}