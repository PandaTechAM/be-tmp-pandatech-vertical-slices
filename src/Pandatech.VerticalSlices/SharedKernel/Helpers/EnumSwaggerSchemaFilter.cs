using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Pandatech.ModularMonolith.ApiGateway.Helpers;

public class EnumSwaggerSchemaFilter : ISchemaFilter
{
   public void Apply(OpenApiSchema schema, SchemaFilterContext context)
   {
      if (context.Type.IsEnum)
      {
         var type = context.Type;

         var list = Enum.GetValues(type)
            .Cast<object>()
            .ToList();

         var enumDescriptions = list.Select(x => $"{x}: {(int)x}").ToList();

         schema.Description = string.Join(", ", enumDescriptions);
      }
   }
}
