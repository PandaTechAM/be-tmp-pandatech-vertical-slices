using Microsoft.OpenApi.Models;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Pandatech.VerticalSlices.SharedKernel.Configurations;

//This class is created because due to some bug /health endpoint is not working in .NET 8. It's included in Microsoft planning.
public class HealthChecksFilter : IDocumentFilter
{
    private const string HealthCheckEndpoint = "/above-board/panda-wellness";
    private static string GroupName => ApiHelper.GroupNameMain;

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var currentDocumentName = context.DocumentName;
             
        if (currentDocumentName != GroupName)
        {
            return;
        }
        
        var pathItem = new OpenApiPathItem();
        var operation = new OpenApiOperation();
        operation.Tags.Add(new OpenApiTag { Name = "above-board" });



        var healthCheckEntrySchema = new OpenApiSchema
        {
           Type = "object",
           Properties = new Dictionary<string, OpenApiSchema>
           {
              { "data", new OpenApiSchema { Type = "object", AdditionalPropertiesAllowed = true } },
              { "duration", new OpenApiSchema { Type = "string" } },
              { "status", new OpenApiSchema { Type = "string" } },
              { "tags", new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string" } } }
           }
        };

        var healthCheckResponseSchema = new OpenApiSchema
        {
           Type = "object",
           Properties = new Dictionary<string, OpenApiSchema>
           {
              { "status", new OpenApiSchema { Type = "string" } },
              { "totalDuration", new OpenApiSchema { Type = "string" } },
              {
                 "entries", new OpenApiSchema
                 {
                    Type = "object",
                    AdditionalPropertiesAllowed = true,
                    AdditionalProperties = healthCheckEntrySchema
                 }
              }
           }
        };

        var response = new OpenApiResponse
        {
           Description = "Health Check Response",
           Content = new Dictionary<string, OpenApiMediaType>
           {
              ["application/json"] = new()
              {
                 Schema = healthCheckResponseSchema
              }
           }
        };

        operation.Responses.Add("200", response);

        pathItem.AddOperation(OperationType.Get, operation);

        swaggerDoc.Paths.Add(HealthCheckEndpoint, pathItem);
    }
}
