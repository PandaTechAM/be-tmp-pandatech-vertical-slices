using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.User.Contracts.GetUser;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.GetUser;

public class GetUserQueryHandler(PostgresContext postgresContext)
   : IQueryHandler<GetUserQuery, GetUserQueryResponse>
{
   public async Task<GetUserQueryResponse> Handle(GetUserQuery request,
      CancellationToken cancellationToken)
   {
      var user = await postgresContext
         .Users
         .FirstOrDefaultAsync(x => x.Id == request.Id && x.Role != UserRole.SuperAdmin,
            cancellationToken);

      NotFoundException.ThrowIfNull(user);

      return GetUserQueryResponse.MapFromEntity(user);
   }
}
