using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.Create;

public record CreateUserV1Command(
  string FullName,
  string Username,
  string Password,
  UserRole UserRole,
  string? Comment) : ICommand;
