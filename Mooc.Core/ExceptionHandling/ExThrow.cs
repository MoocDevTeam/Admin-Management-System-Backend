namespace Mooc.Core.ExceptionHandling;

public static class ExThrow
{
    public static void Throw(string message)
    {
        throw new UserFriendlyException(message, message);
    }

    public static void Throw(string message,string responseMessage)
    {
        throw new UserFriendlyException(message, responseMessage);
    }
   

}
