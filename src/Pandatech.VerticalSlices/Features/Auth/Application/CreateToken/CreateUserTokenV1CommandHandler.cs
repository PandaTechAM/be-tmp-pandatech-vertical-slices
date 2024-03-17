using Pandatech.Crypto;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Features.Auth.Contracts.CreateToken;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.Infrastructure.Contexts;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.CreateToken;

public class CreateUserTokenV1CommandHandler(IConfiguration configuration, PostgresContext dbContext)
   : ICommandHandler<CreateUserTokenV1Command, CreateUserTokenV1CommandResponse>
{
   private const int AccessTokenExpirationMinutes = UserTokenHelpers.AccessTokenExpirationMinutes;

   private readonly int _refreshTokenExpirationMinutes =
      UserTokenHelpers.SetRefreshTokenExpirationMinutes(configuration);

   public async Task<CreateUserTokenV1CommandResponse> Handle(CreateUserTokenV1Command request, CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;

      var accessTokenSignature = Guid.NewGuid().ToString();
      var refreshTokenSignature = Guid.NewGuid().ToString();

      var token = new UserTokenEntity
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

      await dbContext.UserTokens.AddAsync(token, cancellationToken);
      await dbContext.SaveChangesAsync(cancellationToken);

      return CreateUserTokenV1CommandResponse.MapFromEntity(token, accessTokenSignature, refreshTokenSignature);
   }
}
