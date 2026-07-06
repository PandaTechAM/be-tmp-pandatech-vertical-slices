using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdatePasswordForced;

public record UpdatePasswordForcedCommand(string NewPassword) : ICommand;
