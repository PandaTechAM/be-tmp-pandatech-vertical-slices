using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;

public class RequestContext : IRequestContext
{
   public Identity Identity { get; set; } = new();
   public MetaData MetaData { get; set; } = new();
   public bool IsAuthenticated { get; set; } = false;
}
