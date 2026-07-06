using Pandatech.VerticalSlices.Features.Auth.Contracts.RefreshToken;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public record RefreshTokenCommand(string RefreshTokenSignature) : ICommand<RefreshTokenV1CommandResponse>;
