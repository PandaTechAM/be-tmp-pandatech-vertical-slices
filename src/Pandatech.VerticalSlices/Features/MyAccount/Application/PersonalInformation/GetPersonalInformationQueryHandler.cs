using Pandatech.VerticalSlices.Features.MyAccount.Contracts;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.MyAccount.Application.PersonalInformation;

public class GetPersonalInformationQueryHandler(IRequestContext requestContext)
   : IQueryHandler<GetPersonalInformationQuery, GetPersonalInformationQueryResponse>
{
   public Task<GetPersonalInformationQueryResponse> Handle(GetPersonalInformationQuery request,
      CancellationToken cancellationToken)
   {
      return Task.FromResult(GetPersonalInformationQueryResponse.MapFromRequestContext(requestContext));
   }
}
