using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.MyAccount.Application.UpdateOwnPassword;

public record UpdateOwnPasswordCommand(string OldPassword, string NewPassword) : ICommand;
