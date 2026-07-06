using Pandatech.VerticalSlices.Features.Auth.Contracts.CreateToken;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.Auth.Application.CreateToken;

public record CreateTokenCommand(long UserId) : ICommand<CreateTokenCommandResponse>;
