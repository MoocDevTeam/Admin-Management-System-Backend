namespace Mooc.Core.ExceptionHandling;

public class MoocAuthorizationException : MoocException
{
    public override string Code { get; set; } = "100004";
    public MoocAuthorizationException(string message) : base(message)
    {

    }

    public MoocAuthorizationException(string message, string responseMessage) : base(message)
    {
        this.ResponseMessage = responseMessage;
    }
}
