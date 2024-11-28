using Pandatech.VerticalSlices.Domain.Enums;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.Create;

public record CreateUserCommand(
   string FullName,
   string Username,
   string Password,
   UserRole UserRole,
   string? Comment) : ICommand;