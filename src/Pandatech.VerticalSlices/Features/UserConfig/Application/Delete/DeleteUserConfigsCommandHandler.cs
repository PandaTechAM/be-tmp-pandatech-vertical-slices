using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.Delete;

public class DeleteUserConfigsCommandHandler(PostgresContext dbContext, IRequestContext requestContext)
   : ICommandHandler<DeleteUserConfigsCommand>
{
   public async Task Handle(DeleteUserConfigsCommand request, CancellationToken cancellationToken)
   {
      var userConfigs = await dbContext
         .UserConfigs
         .Where(x => x.UserId == requestContext.Identity.UserId
                     && request.Keys
                        .Contains(x.Key))
         .ToListAsync(cancellationToken);

      if (userConfigs.Count != 0)
      {
         dbContext.UserConfigs.RemoveRange(userConfigs);
         await dbContext.SaveChangesAsync(cancellationToken);
      }
   }
}
