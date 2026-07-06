using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokensExceptCurrentSession;

public record RevokeAllTokensExceptCurrentCommand : ICommand;
