using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.User.Contracts.GetById;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.GetById;

public class GetUserByIdV1QueryHandler(PostgresContext postgresContext)
   : IQueryHandler<GetUserByIdV1Query, GetUserByIdV1QueryResponse>
{
   public async Task<GetUserByIdV1QueryResponse> Handle(GetUserByIdV1Query request,
      CancellationToken cancellationToken)
   {
      var user = await postgresContext.Users.FindAsync([request.Id], cancellationToken);

      if (user is null || user.Role == UserRole.SuperAdmin)
      {
         throw new NotFoundException("User not found");
      }

      return GetUserByIdV1QueryResponse.MapFromEntity(user);
   }
}
