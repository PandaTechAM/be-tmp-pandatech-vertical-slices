using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokens;

public record RevokeAllUserTokensV1Command(long UserId) : ICommand;
