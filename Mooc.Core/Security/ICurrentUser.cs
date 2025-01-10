namespace Mooc.Core.Security;

public interface ICurrentUser
{
    /// <summary>
    /// Login or not?
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// User Id
    /// </summary>
    long? Id { get; }

    /// <summary>
    /// User Name
    /// </summary>
    string? Name { get; }
   
}
