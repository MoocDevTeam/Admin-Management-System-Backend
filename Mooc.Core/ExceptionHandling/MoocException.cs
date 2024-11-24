namespace Mooc.Core.ExceptionHandling;

public class MoocException : Exception, IUserFriendlyException
{
    public virtual string Code { get; set; }
    public string ResponseMessage { get; set; }

    public MoocException()
    {

    }

    public MoocException(string message) : base(message)
    {

    }


    public MoocException(string message, string responseMessage) : base(message)
    {
        this.ResponseMessage = responseMessage;
    }

}
