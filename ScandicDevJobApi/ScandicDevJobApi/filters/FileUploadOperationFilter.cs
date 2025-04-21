using System.Linq;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ScandicDevJobApi.filters;
public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Look for any IFormFile parameters
        //var hasFileParam = context.MethodInfo
        //    .GetParameters()
        //    .Any(p => p.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile));
        //if (!hasFileParam) return;
        if (operation.RequestBody != null &&
            context.ApiDescription.HttpMethod == "POST" &&
            context.ApiDescription.ParameterDescriptions.Any())
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["file"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            },
                            Required = new HashSet<string> { "file" }
                        }
                    }
                }
            };
        }



        
    }
}