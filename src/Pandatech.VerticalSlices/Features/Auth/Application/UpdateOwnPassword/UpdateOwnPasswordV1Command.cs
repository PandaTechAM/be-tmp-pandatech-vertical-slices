using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdateOwnPassword;

public record UpdateOwnPasswordV1Command(string OldPassword, string NewPassword) : ICommand;
