using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;

namespace Pandatech.VerticalSlices.SharedKernel.Interfaces;

public interface IRequestContext
{
   public Identity Identity { get; set; }
   public MetaData MetaData { get; set; }
   public bool IsAuthenticated { get; set; }
}