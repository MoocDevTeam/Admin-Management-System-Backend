namespace Mooc.Core.ExceptionHandling;

public interface IUserFriendlyException
{
    public string Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ResponseMessage { get; set; }
}
