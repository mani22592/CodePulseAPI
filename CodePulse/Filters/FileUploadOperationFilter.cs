using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CodePulse.Filters
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile));

            if (fileParams.Any())
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = {
                                ["imageFile"] = new OpenApiSchema { Type = "string", Format = "binary" },
                                ["fileName"] = new OpenApiSchema { Type = "string" },
                                ["title"] = new OpenApiSchema { Type = "string" }
                            },
                            Required = new HashSet<string> { "imageFile", "fileName", "title" }
                        }
                    }
                }
                };
            }
        }
    }
}
