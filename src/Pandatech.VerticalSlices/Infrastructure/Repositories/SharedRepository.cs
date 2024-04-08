using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Infrastructure.Context;

namespace Pandatech.VerticalSlices.Infrastructure.Repositories;

public class SharedRepository(PostgresContext context) : DatabaseOperationsBase(context)
{
   public Task<UserEntity?> GetUserByUsername(string username, CancellationToken cancellationToken)
   {
      return Context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
   }
}
