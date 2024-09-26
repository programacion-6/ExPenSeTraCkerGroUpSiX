using RestApi.Domain;

namespace RestApi.Mappers;

public record CreateIncomeRequest :CreateRequestTemplate<Income>
{
    public required decimal Amount { get; set; }
    public required DateTime Date { get; set; }
    public required string Source { get; set; }
    public Income ToDomain()
    {
        return new Income
        {
            Amount = Amount,
            Date = Date,
            Source = Source
        };
    }
}
