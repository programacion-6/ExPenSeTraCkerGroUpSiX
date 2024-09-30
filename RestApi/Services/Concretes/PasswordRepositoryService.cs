namespace RestApi.Services.Concretes;

public class PasswordRepositoryService
{
    private readonly Dictionary<Guid, string> _passwordResetCodes = new();

    public string Get(Guid guid)
    {
        return _passwordResetCodes[guid];
    }

    public void Add(Guid guid, string password)
    {
        _passwordResetCodes[guid] = password;
    }

    public void Remove(Guid guid)
    {
        _passwordResetCodes.Remove(guid);
    }

    public bool IsEmpty()
    {
        return _passwordResetCodes.Count == 0;
    }

    public bool Contains(Guid guid)
    {
        return _passwordResetCodes.ContainsKey(guid);
    }
}