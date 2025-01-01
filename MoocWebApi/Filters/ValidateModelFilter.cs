﻿using Microsoft.AspNetCore.Mvc.Controllers;
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
                // List to hold errors for multiple fields
                var errorResultsList = new List<ErrorResults>();

                foreach (var item in context.ModelState)
                {
                    if (item.Value.Errors.Count > 0)
                    {
                        var fieldErrors = new ErrorResults
                        {
                            Field = item.Key,
                            Errors = item.Value.Errors.Select(x => x.ErrorMessage).ToList()
                        };

                        errorResultsList.Add(fieldErrors);
                        errorResultsDic.Add(item.Key, fieldErrors.Errors);
                    }
                }

                var apiResponseResult = new ApiResponseResult<List<ErrorResults>>
                {
                    IsSuccess = false,
                    Status = (int)HttpStatusCode.BadRequest,
                    Time = DateTime.Now,
                    // Return the list of field-specific errors
                    Data = errorResultsList
                };
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
