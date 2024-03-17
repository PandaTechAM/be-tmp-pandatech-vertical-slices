using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;

public class RequestContext : IRequestContext
{
   public Identity Identity { get; set; } = null!;
   public MetaData MetaData { get; set; } = null!;
   public bool IsAuthenticated { get; set; } = false;
}
