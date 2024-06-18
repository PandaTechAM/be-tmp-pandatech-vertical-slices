using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdateOwnPassword;

public record UpdateOwnPasswordCommand(string OldPassword, string NewPassword) : ICommand;
