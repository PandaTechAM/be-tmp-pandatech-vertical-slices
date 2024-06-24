using GridifyExtensions.Extensions;
using GridifyExtensions.Models;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.GetColumnDistinctValues;

public class GetUserColumnDistinctValuesQueryHandler(PostgresContext dbContext)
   : IQueryHandler<GetUserColumnDistinctValuesQuery, CursoredResponse<object>>
{
   public Task<CursoredResponse<object>> Handle(GetUserColumnDistinctValuesQuery request,
      CancellationToken cancellationToken)
   {
      return dbContext
         .Users
         .Where(u => u.Role != UserRole.SuperAdmin)
         .ColumnDistinctValuesAsync(request, cancellationToken: cancellationToken);
   }
}
