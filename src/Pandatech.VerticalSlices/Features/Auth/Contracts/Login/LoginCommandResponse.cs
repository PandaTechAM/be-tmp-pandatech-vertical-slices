using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Contracts.CreateToken;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Login;

public class LoginCommandResponse
{
    public long UserId { get; set; }

    public bool ForcePasswordChange { get; set; }
    public UserRole UserRole { get; set; }
    public required string AccessTokenSignature { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public required string RefreshTokenSignature { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }

    public static LoginCommandResponse MapFromEntity(CreateTokenCommandResponse token,
        UserRole userRole,
        bool forcePasswordChange)
    {
        return new LoginCommandResponse
        {
            UserId = token.UserId,
            ForcePasswordChange = forcePasswordChange,
            UserRole = userRole,
            AccessTokenSignature = token.AccessTokenSignature,
            AccessTokenExpiration = token.AccessTokenExpiresAt,
            RefreshTokenSignature = token.RefreshTokenSignature,
            RefreshTokenExpiration = token.RefreshTokenExpiresAt
        };
    }
}
