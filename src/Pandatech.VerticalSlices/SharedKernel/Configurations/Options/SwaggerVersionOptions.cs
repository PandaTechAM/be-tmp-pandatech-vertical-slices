namespace Pandatech.VerticalSlices.SharedKernel.Configurations.Options;

public class SwaggerVersionOptions
{
   public string Title { get; set; } = null!;
   public string Description { get; set; } = null!;

   public bool Separate { get; set; }
   public string? RoutePrefix { get; set; }
}