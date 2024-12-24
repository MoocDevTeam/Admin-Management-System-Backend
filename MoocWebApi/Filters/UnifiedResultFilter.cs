using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Mooc.Core.WrapperResult;
using System.Net;

namespace MoocWebApi.Filters
{
    public class UnifiedResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null)
                return;

            var controllerWrapper = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(NoWrapperResultAttribute), true);
            if (controllerWrapper.Any())
                return;

            var actionWrappers = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(NoWrapperResultAttribute), true);
            if (actionWrappers.Any())
                return;


            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value == null)
                {
                    return;
                }
                else
                {
                    if (objectResult.DeclaredType == null || objectResult.DeclaredType == typeof(ApiResponseResult))
                    {
                        return;
                    }

                    if (objectResult.DeclaredType.IsGenericType && objectResult.DeclaredType.GetGenericTypeDefinition() == typeof(ApiResponseResult<>))
                    {
                        return;
                    }

                    var apiResponseResult = new ApiResponseResult<object>();
                    apiResponseResult.IsSuccess = (context.HttpContext.Response.StatusCode >= (int)HttpStatusCode.OK && context.HttpContext.Response.StatusCode <= (int)HttpStatusCode.NoContent);
                    apiResponseResult.Status = context.HttpContext.Response.StatusCode;
                    apiResponseResult.Time = DateTime.Now;
                    apiResponseResult.Data = objectResult.Value;

                    context.Result = new JsonResult(apiResponseResult);
                }
            }
        }
    }
}
