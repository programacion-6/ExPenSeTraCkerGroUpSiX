namespace RestApi.Services.interfaces;

public interface IBaseService<T> where T : class
{
    public Task<T> CreateAsync(T item);
    public Task<List<T>> ReadAsync();
    public Task<T?> ReadAsync(Guid id);
    public Task<bool> UpdateAsync(Guid id, T updatedExpense);
    public Task<bool> DeleteAsync(Guid id);

}