using Pandatech.VerticalSlices.Features.Auth.Contracts.IdentityState;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.IdentityState;

public class GetIdentityStateQueryHandler(IRequestContext requestContext)
   : IQueryHandler<GetIdentityStateQuery, IdentityStateCommandResponse>
{
   public Task<IdentityStateCommandResponse> Handle(GetIdentityStateQuery request,
      CancellationToken cancellationToken)
   {
      return Task.FromResult(IdentityStateCommandResponse.MapFromIdentity(requestContext.Identity));
   }
}
