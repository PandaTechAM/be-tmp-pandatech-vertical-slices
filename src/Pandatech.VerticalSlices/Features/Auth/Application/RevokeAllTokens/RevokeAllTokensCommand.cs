using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokens;

public record RevokeAllTokensCommand(long UserId) : ICommand;