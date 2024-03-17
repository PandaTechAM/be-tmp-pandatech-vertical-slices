using Pandatech.VerticalSlices.Features.Auth.Contracts.RefreshToken;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public record RefreshUserTokenV1Command(string RefreshTokenSignature) : ICommand<RefreshUserTokenV1CommandResponse>;
