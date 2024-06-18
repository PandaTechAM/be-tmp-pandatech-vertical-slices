using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdatePasswordForced;

public record UpdatePasswordForcedCommand(string NewPassword) : ICommand;
