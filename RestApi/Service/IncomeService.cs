
using RestApi.Domain;
using RestApi.Persistence;

namespace RestApi.Service;

public class IncomeService(ApplicationDbContext context) : BaseService<Income>
{
    private readonly ApplicationDbContext _Context = context;

    public bool Create(Income item)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Income GetItem(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Income> GetList()
    {
        throw new NotImplementedException();
    }

    public List<Income> Read()
    {
        throw new NotImplementedException();
    }

    public bool Update(Guid Id, Income item)
    {
        throw new NotImplementedException();
    }
}