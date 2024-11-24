using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoocWebApi.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            this._logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // We can't do anything if the response has already started, just abort.
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("An exception occurred, but response has already started!");
                    throw;
                }

                await HandleAndWrapExceptionAsync(context, ex);
            }

        }

        private async Task HandleAndWrapExceptionAsync(HttpContext httpContext, Exception exception)
        {
            this._logger.LogError(exception, "ExceptionHandlingMiddleware");

            httpContext.Response.Clear();
            var statusCode = (int)HttpExceptionUtil.GetStatusCode(httpContext, exception);
            httpContext.Response.StatusCode = statusCode;
            var apiResponseResult = HttpExceptionUtil.GetApiResponseResult(exception);

            //var serializeOptions = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //    //PropertyNamingPolicy = new UpperCaseNamingPolicy(),
            //    WriteIndented = true // 
            //};

            var json = JsonSerializer.Serialize(apiResponseResult, serializeOptions);
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(json);
        }

        private static JsonSerializerOptions serializeOptions = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //PropertyNamingPolicy = new UpperCaseNamingPolicy(),
            WriteIndented = true //
        };
    }
}
