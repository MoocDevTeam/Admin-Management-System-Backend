namespace Mooc.Core.ExceptionHandling;

public class MoocValidationException : MoocException
{
    public override string Code { get; set; } = "100003";

    public MoocValidationException(string message) : base(message)
    {

    }


    public MoocValidationException( string message, string responseMessage) : base(message)
    {
        this.ResponseMessage = responseMessage;
    }
}
