using RestApi.Persistence;

namespace RestApi.Service;

public interface BaseService<T> where T : class
{
    public bool Create(T item);
    public List<T> GetList();
    public T GetItem(Guid id);
    public bool Delete(Guid id);
    public bool Update(Guid Id, T item);
}