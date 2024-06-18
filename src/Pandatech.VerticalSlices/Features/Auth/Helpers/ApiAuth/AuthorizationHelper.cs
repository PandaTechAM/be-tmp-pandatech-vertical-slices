using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Helpers.ApiAuth;

public static class AuthorizationHelper
{
   public static void Authorize(Identity identity, UserRole minimumRole, bool isForcedToChange)
   {
      if (identity.UserRole > minimumRole)
      {
         throw new ForbiddenException();
      }

      switch (identity.ForcePasswordChange)
      {
         case true when !isForcedToChange:
            throw new ForbiddenException(ErrorMessages.YourPasswordIsExpired);
         case false when isForcedToChange:
            throw new ForbiddenException(ErrorMessages.YourPasswordIsNotExpired);
      }
   }
}
