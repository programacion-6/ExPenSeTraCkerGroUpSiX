using System.Data;

namespace RestApi.Persistence.DataBase;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}