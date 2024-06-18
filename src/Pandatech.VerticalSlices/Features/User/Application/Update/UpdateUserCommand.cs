using System.Text.Json.Serialization;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.Update;

public class UpdateUserCommand : ICommand
{
   [JsonIgnore] public long Id { get; set; }

   public string Username { get; set; } = null!;
   public string FullName { get; set; } = null!;
   public UserRole Role { get; set; }
   public string? Comment { get; set; }
}