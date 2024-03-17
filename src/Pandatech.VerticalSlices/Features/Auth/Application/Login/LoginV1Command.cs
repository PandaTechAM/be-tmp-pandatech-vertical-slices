using Pandatech.VerticalSlices.Features.Auth.Contracts.Login;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Login;

public record LoginV1Command(string Username, string Password) : ICommand<LoginV1CommandResponse>;
