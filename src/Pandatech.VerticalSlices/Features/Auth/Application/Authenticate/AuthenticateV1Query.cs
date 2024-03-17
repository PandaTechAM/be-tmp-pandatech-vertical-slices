using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Authenticate;

public record AuthenticateV1Query(string AccessTokenSignature) : IQuery<Identity>;

