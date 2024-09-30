namespace RestApi.Services;

public class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message)
    { }
}