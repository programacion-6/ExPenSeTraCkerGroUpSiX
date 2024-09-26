namespace RestApi.Mappers;

public interface CreateRequestTemplate<T>
{
    T ToDomain();
}
