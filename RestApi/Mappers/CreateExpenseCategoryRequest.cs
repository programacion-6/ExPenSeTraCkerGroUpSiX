using RestApi.Domain;

namespace RestApi.Mappers
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