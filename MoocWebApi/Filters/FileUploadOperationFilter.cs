using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Reflection;
using System.Text;
namespace MoocWebApi.Filters;
public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var formParameters = context.MethodInfo.GetParameters()
            .Where(p =>
                p.ParameterType == typeof(IFormFile) ||
                p.CustomAttributes.Any(attr => attr.AttributeType == typeof(FromFormAttribute)));

        if (formParameters.Any())
        {
            var schema = new OpenApiSchema
            {
                Type = "object",
                Properties = formParameters.ToDictionary(
                    p => p.Name, 
                    p => new OpenApiSchema
                    {
                        Type = p.ParameterType == typeof(IFormFile) ? "string" : "string",
                        Format = p.ParameterType == typeof(IFormFile) ? "binary" : null 
                    })
            };

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
            {
                {
                    "multipart/form-data",
                    new OpenApiMediaType
                    {
                        Schema = schema
                    }
                }
            }
            };
        }
    }

}
