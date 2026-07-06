using Pandatech.VerticalSlices.Features.Auth.Contracts.IdentityState;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.Auth.Application.IdentityState;

public class GetIdentityStateQuery : IQuery<IdentityStateCommandResponse>;
