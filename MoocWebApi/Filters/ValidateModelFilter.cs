using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Mooc.Core.WrapperResult;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoocWebApi.Filters
{
    public class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null)
                return;


            if (!context.ModelState.IsValid)
            {
                var errorResultsDic = new Dictionary<string, List<string>>();
                var errorResults = new ErrorResults();
                foreach (var item in context.ModelState)
                {
                    if (item.Value.Errors.Count > 0)
                    {
                        errorResults.Field = item.Key;
                        errorResults.Errors.AddRange(item.Value.Errors.Select(x => x.ErrorMessage));
                        errorResultsDic.Add(item.Key, item.Value.Errors.Select(x => x.ErrorMessage).ToList());
                    }
                }
                var apiResponseResult = new ApiResponseResult<ErrorResults>();
                apiResponseResult.IsSuccess = true;
                apiResponseResult.Status = (int)HttpStatusCode.BadRequest;
                apiResponseResult.Time = DateTime.Now;
                apiResponseResult.Data = errorResults;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //var serializeOptions = new JsonSerializerOptions
                //{
                //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                //    //PropertyNamingPolicy = new UpperCaseNamingPolicy(),
                //    WriteIndented = true //
                //};
                //var json = JsonSerializer.Serialize(apiResponseResult, serializeOptions);
                context.Result = new JsonResult(apiResponseResult);


            }
        }
    }


    public class ErrorResults
    {
        public string Field { get; set; }

        public List<string> Errors { get; set; }=new List<string>();
    }

}
