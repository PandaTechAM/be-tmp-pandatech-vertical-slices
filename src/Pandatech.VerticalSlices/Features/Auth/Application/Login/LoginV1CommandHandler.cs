using MediatR;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Application.CreateToken;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Login;
using Pandatech.VerticalSlices.Infrastructure.Contexts;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Login;

public class LoginV1CommandHandler(PostgresContext dbContext, Argon2Id argon2Id, ISender sender)
   : ICommandHandler<LoginV1Command, LoginV1CommandResponse>
{
   public async Task<LoginV1CommandResponse> Handle(LoginV1Command request, CancellationToken cancellationToken)
   {
      var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

      if (user is null || user.Status != UserStatus.Active ||
          !argon2Id.VerifyHash(request.Password, user.PasswordHash))
      {
         throw new BadRequestException("invalid_username_or_password");
      }

      var userToken = await sender.Send(new CreateUserTokenV1Command(user.Id), cancellationToken);

      return LoginV1CommandResponse.MapFromEntity(userToken, user.Role, user.ForcePasswordChange);
   }
}
