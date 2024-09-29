using Microsoft.EntityFrameworkCore;
using RestApi.Domain;
using RestApi.Mappers.Interfaces;
using RestApi.Persistence.DataBase;

namespace RestApi.Mappers.Concretes;

public class CreateExpenseRequest : CreateRequestTemplate<Expense>
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }

    public Expense ToDomain()
    {
        return new Expense
        {
            UserId = UserContext.CurrentUserId,
            Amount = Amount,
            Date = Date,
            Description = Description,
            Category = null,
        };
    }
}
