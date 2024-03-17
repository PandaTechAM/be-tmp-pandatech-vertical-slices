using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Features.User.Contracts;

public record UserV1Response(
  long Id,
  string Username,
  string FullName,
  UserRole UserRole,
  UserStatus Status,
  bool ForcePasswordChange,
  DateTime CreatedAt,
  DateTime UpdatedAt,
  string? Comment)
{
  public static UserV1Response MapFromEntity(UserEntity entity)
  {
    return new UserV1Response(
      entity.Id,
      entity.Username,
      entity.FullName,
      entity.Role,
      entity.Status,
      entity.ForcePasswordChange,
      entity.CreatedAt,
      entity.UpdatedAt,
      entity.Comment);
  }
}
