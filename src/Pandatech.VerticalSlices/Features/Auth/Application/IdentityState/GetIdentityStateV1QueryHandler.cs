using Pandatech.VerticalSlices.Features.Auth.Contracts.IdentityState;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.IdentityState;

public class GetIdentityStateV1QueryHandler(IRequestContext requestContext)
   : IQueryHandler<GetIdentityStateV1Query, IdentityStateV1CommandResponse>
{
   public Task<IdentityStateV1CommandResponse> Handle(GetIdentityStateV1Query request, CancellationToken cancellationToken)
   {
      return Task.FromResult(IdentityStateV1CommandResponse.MapFromIdentity(requestContext.Identity));
   }
}
