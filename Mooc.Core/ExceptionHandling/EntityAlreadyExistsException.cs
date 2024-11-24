namespace Mooc.Core.ExceptionHandling;

public class EntityAlreadyExistsException : MoocException
{
    public override string Code { get; set; } = "100002";
    public EntityAlreadyExistsException(string message) : base(message)
    {

    }

    public EntityAlreadyExistsException( string message, string responseMessage) : base(message)
    {
        this.ResponseMessage = responseMessage;
    }
}
