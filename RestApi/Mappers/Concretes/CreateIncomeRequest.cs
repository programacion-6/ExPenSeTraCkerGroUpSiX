using RestApi.Domain;
using RestApi.Mappers.Interfaces;
using RestApi.Persistence.DataBase;

namespace RestApi.Mappers.Concretes;

public record CreateIncomeRequest : CreateRequestTemplate<Income>
{
    public required decimal Amount { get; set; }
    public required DateTime Date { get; set; }
    public required string Source { get; set; }
    public Income ToDomain()
    {
        return new Income
        {
            UserId = UserContext.CurrentUserId,
            Amount = Amount,
            Date = Date,
            Source = Source
        };
    }
}
