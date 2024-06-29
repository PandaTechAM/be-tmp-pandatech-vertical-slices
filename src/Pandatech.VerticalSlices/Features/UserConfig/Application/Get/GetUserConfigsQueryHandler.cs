using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.Get;

public class GetUserConfigsQueryHandler(PostgresContext dbContext, IRequestContext requestContext)
   : IQueryHandler<GetUserConfigsQuery, Dictionary<string, string>>
{
   public Task<Dictionary<string, string>> Handle(GetUserConfigsQuery request, CancellationToken cancellationToken)
   {
      return dbContext
         .UserConfigs
         .Where(x => x.UserId == requestContext.Identity.UserId
                     && request.Keys
                        .Contains(x.Key))
         .AsNoTracking()
         .ToDictionaryAsync(x => x.Key, x => x.Value, cancellationToken);
   }
}
