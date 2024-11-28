using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokens;

public record RevokeAllTokensCommand(long UserId) : ICommand;