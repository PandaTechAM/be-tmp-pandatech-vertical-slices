using Pandatech.VerticalSlices.Features.Auth.Contracts.CreateToken;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.CreateToken;

public record CreateTokenCommand(long UserId) : ICommand<CreateTokenCommandResponse>;