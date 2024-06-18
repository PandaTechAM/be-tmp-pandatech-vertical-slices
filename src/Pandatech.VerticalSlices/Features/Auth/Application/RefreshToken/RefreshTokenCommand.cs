using Pandatech.VerticalSlices.Features.Auth.Contracts.RefreshToken;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public record RefreshTokenCommand(string RefreshTokenSignature) : ICommand<RefreshTokenV1CommandResponse>;