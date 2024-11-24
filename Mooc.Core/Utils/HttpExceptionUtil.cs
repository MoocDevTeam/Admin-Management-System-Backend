using Microsoft.AspNetCore.Http;
using Mooc.Core.ExceptionHandling;
using Mooc.Core.WrapperResult;
using System.Net;

namespace Mooc.Core.Utils;

public static class HttpExceptionUtil
{
    public static HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
    {

        if (exception is MoocAuthorizationException)
        {
            return httpContext.User.Identity.IsAuthenticated
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
        }

        if (exception is MoocValidationException || exception is EntityAlreadyExistsException)
        {
            return HttpStatusCode.BadRequest;
        }

        if (exception is EntityNotFoundException)
        {
            return HttpStatusCode.NotFound;
        }

        if (exception is NotImplementedException)
        {
            return HttpStatusCode.NotImplemented;
        }

        if (exception is UserFriendlyException)
        {
            return HttpStatusCode.BadRequest;
        }

        return HttpStatusCode.InternalServerError;
    }

    public static ApiResponseResult GetApiResponseResult(Exception exception)
    {
        ApiResponseResult apiResponseResult = new ApiResponseResult();
        apiResponseResult.IsSuccess = false;

        if (exception is MoocValidationException moocValidationException)
        {
            apiResponseResult.Status = (int)HttpStatusCode.BadRequest;
            apiResponseResult.Message = moocValidationException.ResponseMessage;
        }

        else if (exception is EntityAlreadyExistsException entityAlreadyExistsException)
        {
            apiResponseResult.Status = (int)HttpStatusCode.BadRequest;
            apiResponseResult.Message = entityAlreadyExistsException.ResponseMessage;
        }
        else if (exception is EntityNotFoundException entityNotFoundException)
        {
            apiResponseResult.Status = (int)HttpStatusCode.NotFound;
            apiResponseResult.Message = entityNotFoundException.ResponseMessage;
        }
        else if (exception is NotImplementedException)
        {
            apiResponseResult.Status = (int)HttpStatusCode.NotImplemented;
            apiResponseResult.Message = exception.Message;

        }
        else if (exception is UserFriendlyException userFriendlyException)
        {
            apiResponseResult.Status = (int)HttpStatusCode.BadRequest;
            apiResponseResult.Message = userFriendlyException.ResponseMessage;
        }
        else
        {
            apiResponseResult.Status = (int)HttpStatusCode.InternalServerError;
            apiResponseResult.Message = exception.Message;
        }

        apiResponseResult.Time = DateTime.Now;
        return apiResponseResult;
    }
}
