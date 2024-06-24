using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.MyAccount.Contracts;

public record GetPersonalInformationQueryResponse(
   string Username,
   string FullName,
   UserRole UserRole,
   DateTime CreatedAt)
{
   public static GetPersonalInformationQueryResponse MapFromRequestContext(IRequestContext requestContext) =>
      new(requestContext.Identity.Username, requestContext.Identity.FullName, requestContext.Identity.UserRole,
         requestContext.Identity.CreatedAt);
}
