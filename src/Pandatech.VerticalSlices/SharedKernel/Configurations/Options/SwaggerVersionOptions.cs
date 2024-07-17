namespace Pandatech.VerticalSlices.SharedKernel.Configurations.Options;

public class SwaggerVersionOptions
{
   public required string Title { get; set; }
   public required string Description { get; set; }

   public bool Separate { get; set; }
   public string? RoutePrefix { get; set; }
}