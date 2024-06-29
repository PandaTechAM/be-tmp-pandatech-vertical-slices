using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.CreateOrUpdate;

public class CreateOrUpdateUserConfigCommandHandler(PostgresContext dbContext, IRequestContext requestContext)
   : ICommandHandler<CreateOrUpdateUserConfigCommand>
{
   public async Task Handle(CreateOrUpdateUserConfigCommand request, CancellationToken cancellationToken)
   {
      var keys = request.Configs.Select(x => x.Key).ToList();

      var userConfigEntities = await dbContext
         .UserConfigs
         .Where(x => x.UserId == requestContext.Identity.UserId && keys.Contains(x.Key))
         .ToListAsync(cancellationToken);

      foreach (var requestedUserConfig in request.Configs)
      {
         var userConfigEntity = userConfigEntities.Find(x => x.Key == requestedUserConfig.Key);

         if (userConfigEntity is null)
         {
            userConfigEntity = new Domain.Entities.UserConfig
            {
               Key = requestedUserConfig.Key,
               Value = requestedUserConfig.Value,
               UserId = requestContext.Identity.UserId,
               CreatedByUserId = requestContext.Identity.UserId
            };

            dbContext.UserConfigs.Add(userConfigEntity);
         }
         else
         {
            if (userConfigEntity.Value != requestedUserConfig.Value)
            {
               userConfigEntity.Value = requestedUserConfig.Value;
            }
         }
      }

      await dbContext.SaveChangesAsync(cancellationToken);
   }
}
