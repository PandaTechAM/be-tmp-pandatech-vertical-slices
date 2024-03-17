using Pandatech.VerticalSlices.Features.Auth.Contracts.CreateToken;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.CreateToken;

public record CreateUserTokenV1Command(long UserId) : ICommand<CreateUserTokenV1CommandResponse>;
