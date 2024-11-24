namespace Mooc.Core.ExceptionHandling;

public class UserFriendlyException : MoocException
{
    public override string Code { get; set; } = "100000";

    public UserFriendlyException(string message) : base(message)
    {

    }

    public UserFriendlyException(string message, string responseMessage) : base(message)
    {
        this.ResponseMessage = responseMessage;
    }



}
