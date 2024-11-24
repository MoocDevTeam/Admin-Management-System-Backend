namespace Mooc.Core.Utils;

public class BCryptUtil
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Verify(string password, string hashPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }
}
