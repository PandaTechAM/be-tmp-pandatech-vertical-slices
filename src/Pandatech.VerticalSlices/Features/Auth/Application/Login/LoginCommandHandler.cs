using MediatR;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Application.CreateToken;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Login;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Login;

public class LoginCommandHandler(PostgresContext dbContext, Argon2Id argon2Id, ISender sender)
   : ICommandHandler<LoginCommand, LoginCommandResponse>
{
   public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
   {
      var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

      if (user is null || user.Status != UserStatus.Active ||
          !argon2Id.VerifyHash(request.Password, user.PasswordHash))
      {
         throw new BadRequestException(ErrorMessages.InvalidCredentials);
      }

      var token = await sender.Send(new CreateTokenCommand(user.Id), cancellationToken);

      return LoginCommandResponse.MapFromEntity(token, user.Role, user.ForcePasswordChange);
   }
}
