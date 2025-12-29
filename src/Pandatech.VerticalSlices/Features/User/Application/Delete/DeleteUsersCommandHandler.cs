using EFCore.AuditBase;
using GridifyExtensions.Extensions;
using GridifyExtensions.Models;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.Delete;

public class DeleteUsersCommandHandler(PostgresContext postgresContext, IRequestContext requestContext)
   : ICommandHandler<DeleteUsersCommand>
{
   public Task Handle(DeleteUsersCommand request, CancellationToken cancellationToken)
   {
      var filterModel = new GridifyQueryModel
      {
         Page = 1,
         PageSize = 1,
         OrderBy = null,
         Filter = request.Filter
      };

      return postgresContext
             .Users
             .Where(x => x.Role != UserRole.SuperAdmin)
             .ApplyFilter(filterModel)
             .ExecuteSoftDeleteAsync(requestContext.Identity.UserId, ct: cancellationToken);
   }
}