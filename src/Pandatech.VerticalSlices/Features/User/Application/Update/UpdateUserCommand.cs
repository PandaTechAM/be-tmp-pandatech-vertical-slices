using System.Text.Json.Serialization;
using Pandatech.VerticalSlices.Domain.Enums;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.Update;

public class UpdateUserCommand : ICommand
{
   [JsonIgnore]
   public long Id { get; set; }

   public required string Username { get; set; }
   public required string FullName { get; set; }
   public UserRole Role { get; set; }
   public string? Comment { get; set; }
}