using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.User.Contracts.GetById;
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

      if (user is null)
      {
         throw new NotFoundException();
      }

      return GetUserQueryResponse.MapFromEntity(user);
   }
}
