using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdatePasswordForced;

public record UpdatePasswordForcedV1Command(string NewPassword) : ICommand;
