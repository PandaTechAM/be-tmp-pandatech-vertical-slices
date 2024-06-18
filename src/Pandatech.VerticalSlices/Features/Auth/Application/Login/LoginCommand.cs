using Pandatech.VerticalSlices.Features.Auth.Contracts.Login;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Login;

public record LoginCommand(string Username, string Password) : ICommand<LoginCommandResponse>;
