using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Helpers;

public static class AuthorizationHelper
{
   public static void Authorize(Identity identity, UserRole minimumRole, bool isForcedToChange)
   {
      if (identity.UserRole > minimumRole)
      {
         throw new ForbiddenException("you_are_not_authorized");
      }

      switch (identity.ForcePasswordChange)
      {
         case true when !isForcedToChange:
            throw new ForbiddenException("you_need_to_change_your_password");
         case false when isForcedToChange:
            throw new ForbiddenException("your_password_is_not_expired");
      }
   }
}
