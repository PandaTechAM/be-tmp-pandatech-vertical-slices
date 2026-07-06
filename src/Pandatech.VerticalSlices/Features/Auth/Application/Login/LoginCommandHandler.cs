using MediatR;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto.Helpers;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Application.CreateToken;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Login;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using ResponseCrafter.HttpExceptions;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Login;

public class LoginCommandHandler(PostgresContext dbContext, ISender sender)
    : ICommandHandler<LoginCommand, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username.ToLower(),
            cancellationToken);


        if (user is null || user.Status != UserStatus.Active ||
            !Argon2Id.VerifyHash(request.Password, user.PasswordHash))
        {
            throw new BadRequestException(ErrorMessages.InvalidCredentials);
        }

        var token = await sender.Send(new CreateTokenCommand(user.Id), cancellationToken);

        return LoginCommandResponse.MapFromEntity(token, user.Role, user.ForcePasswordChange);
    }
}
