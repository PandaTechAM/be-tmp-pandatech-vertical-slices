using Pandatech.Crypto;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Features.Auth.Contracts.CreateToken;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.CreateToken;

public class CreateTokenCommandHandler(IConfiguration configuration, PostgresContext dbContext)
   : ICommandHandler<CreateTokenCommand, CreateTokenCommandResponse>
{
   private const int AccessTokenExpirationMinutes = TokenHelpers.AccessTokenExpirationMinutes;

   private readonly int _refreshTokenExpirationMinutes =
      TokenHelpers.SetRefreshTokenExpirationMinutes(configuration);

   public async Task<CreateTokenCommandResponse> Handle(CreateTokenCommand request,
      CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;

      var accessTokenSignature = Guid.NewGuid().ToString();
      var refreshTokenSignature = Guid.NewGuid().ToString();

      var token = new Token
      {
         UserId = request.UserId,
         AccessTokenHash = Sha3.Hash(accessTokenSignature),
         RefreshTokenHash = Sha3.Hash(refreshTokenSignature),
         AccessTokenExpiresAt = now.AddMinutes(AccessTokenExpirationMinutes),
         RefreshTokenExpiresAt = now.AddMinutes(_refreshTokenExpirationMinutes),
         InitialRefreshTokenCreatedAt = now,
         CreatedAt = now,
         UpdatedAt = now
      };

      await dbContext.Tokens.AddAsync(token, cancellationToken);
      await dbContext.SaveChangesAsync(cancellationToken);

      return CreateTokenCommandResponse.MapFromEntity(token, accessTokenSignature, refreshTokenSignature);
   }
}