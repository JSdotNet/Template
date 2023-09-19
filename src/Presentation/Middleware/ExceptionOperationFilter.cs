using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace SolutionTemplate.Presentation.Api.Middleware;

internal sealed class ExceptionOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Any request can return a 400 or 500 response. BadRequest can come from fluent validation.
        // 500 occurs when an unexpected exception is thrown.
        operation.Responses.TryAdd("400", new OpenApiResponse
        {
            Description = "BadRequest",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                }
            }
        });
        operation.Responses.TryAdd("500", new OpenApiResponse
        {
            Description = "InternalServerError",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                }
            }
        });

        // When security is add these response should probably be added
        //operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        //operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });


        var method = context.MethodInfo.GetCustomAttributes(true).OfType<HttpMethodAttribute>().SingleOrDefault();

        if (method is HttpGetAttribute)
        {
            operation.Responses.TryAdd("200", new OpenApiResponse
            {
                Description = "OK",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(string), context.SchemaRepository)
                    }
                }
            });
        }

        // Any request that retrieves a resource or updates a resource should return a 404 if the resource is not found.
        if (method is HttpGetAttribute || method is HttpDeleteAttribute || method is HttpPatchAttribute || method is HttpPutAttribute)
        {
            operation.Responses.TryAdd("404", new OpenApiResponse
            {
                Description = "NotFound",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                    }
                }
            });
        }

        // Create call should return a 201 or 409 response. 409 occurs when the resource already exists.
        if (method is HttpPostAttribute)
        {
            operation.Responses.TryAdd("201", new OpenApiResponse
            {
                Description = "Created",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(string), context.SchemaRepository)
                    }
                }
            });
            operation.Responses.TryAdd("409", new OpenApiResponse
            {
                Description = "Conflict",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                    }
                }
            });
        }

        // Update call should return a 204 or 404 response when successful.
        if (method is HttpDeleteAttribute || method is HttpPatchAttribute || method is HttpPutAttribute)
        {
            operation.Responses.TryAdd("204", new OpenApiResponse { Description = "NoContent" });
        }
    }
}
