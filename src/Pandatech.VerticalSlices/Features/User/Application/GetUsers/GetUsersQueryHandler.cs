using GridifyExtensions.Extensions;
using GridifyExtensions.Models;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.User.Contracts.GetById;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.GetUsers;

public class GetUsersQueryHandler(PostgresContext dbContext)
   : IQueryHandler<GetUsersQuery, PagedResponse<GetUserQueryResponse>>
{
   public Task<PagedResponse<GetUserQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
   {
      return dbContext
         .Users
         .Where(u => u.Role != UserRole.SuperAdmin)
         .OrderBy(x => x.FullName)
         .FilterOrderAndGetPagedAsync(request,
            x => new GetUserQueryResponse
            {
               Id = x.Id,
               Username = x.Username,
               FullName = x.FullName,
               Role = x.Role,
               Status = x.Status,
               Comment = x.Comment
            }, cancellationToken: cancellationToken);
   }
}
