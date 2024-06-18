using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Auth;

public record AuthQuery(
   HttpContext HttpContext,
   UserRole MinimalUserRole,
   bool Anonymous,
   bool ForcedToChangePassword,
   bool IgnoreClientType)
   : IQuery;
