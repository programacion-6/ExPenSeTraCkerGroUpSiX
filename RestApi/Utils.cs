namespace RestApi;

public class Utils
{
    public static string RandomCode()
    {
        return $"{new Random().Next(100000, 1000000)}";
    }
}