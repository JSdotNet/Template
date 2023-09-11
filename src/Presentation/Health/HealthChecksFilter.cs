//using Microsoft.Extensions.Diagnostics.HealthChecks;
//using Microsoft.OpenApi.Models;

//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace SolutionTemplate.WebApi.Health;

//public sealed class HealthChecksFilter : IDocumentFilter
//{
//    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
//    {
//        var schema = context.SchemaGenerator.GenerateSchema(typeof(HealthReport), context.SchemaRepository);

//        var healthyResponse = new OpenApiResponse();
//        healthyResponse.Content.Add("application/json", new OpenApiMediaType { Schema = schema });
//        healthyResponse.Description = "API service is healthy";

//        var unhealthyResponse = new OpenApiResponse();
//        unhealthyResponse.Content.Add("application/json", new OpenApiMediaType { Schema = schema });
//        unhealthyResponse.Description = "API service is not healthy";

//        var operation = new OpenApiOperation { Description = "Returns the health status of this service", };
//        operation.Tags.Add(new OpenApiTag { Name = "Health Check API" });
//        operation.Responses.Add("200", healthyResponse);
//        operation.Responses.Add("500", unhealthyResponse);


//        operation.Parameters.Add(new()
//        {
//            Name = "customParam",
//            In = ParameterLocation.Query,
//            Required = false,
//            Description = "If this parameter is true, ....",
//            Schema = new()
//            {
//                Type = "boolean"
//            }
//        });

//        var pathItem = new OpenApiPathItem();
//        pathItem.AddOperation(OperationType.Get, operation);

//        swaggerDoc.Paths.Add("/_health/live", pathItem);
//        swaggerDoc.Paths.Add("/_health/state", pathItem);
//        swaggerDoc.Paths.Add("/_health/details", pathItem);
//    }
//}


//public sealed class PresentationFilter : IDocumentFilter
//{

//    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
//    {
//        var schema = context.SchemaGenerator.GenerateSchema(typeof(HealthReport), context.SchemaRepository);

//        if (swaggerDoc.Paths.Count <= 0)
//        {
//            return;
//        }
//        foreach (var path in swaggerDoc.Paths.Values)
//        {
//            //ToLower(path.Parameters);

//            //// Edit this list if you want other operations.
//            //var operations = new List<Operation>
//            //{
//            //    path.Get,
//            //    path.Post,
//            //    path.Put
//            //};
//            //operations.FindAll(x => x != null)
//            //    .ForEach(x => ToLower(x.Parameters));
//        }
//    }
//}