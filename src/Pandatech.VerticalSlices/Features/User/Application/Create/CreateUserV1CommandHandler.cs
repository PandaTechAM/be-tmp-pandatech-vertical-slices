using Pandatech.Crypto;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Infrastructure.Contexts;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.Create;

public class CreateUserV1CommandHandler(PostgresContext dbContext, Argon2Id argon)
  : ICommandHandler<CreateUserV1Command>
{
  public async Task Handle(CreateUserV1Command request, CancellationToken cancellationToken)
  {
    var passwordHash = argon.HashPassword(request.Password);
    var user = new UserEntity
    {
      Username = request.Username.ToLower(),
      FullName = request.FullName,
      PasswordHash = passwordHash,
      Role = request.UserRole,
      Comment = request.Comment
    };
    await dbContext.Users.AddAsync(user, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
  }
}
