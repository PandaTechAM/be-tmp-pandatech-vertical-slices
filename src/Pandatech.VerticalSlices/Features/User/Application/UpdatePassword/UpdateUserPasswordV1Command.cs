using System.Text.Json.Serialization;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdatePassword;

public class UpdateUserPasswordV1Command : ICommand
{
   [JsonIgnore] public long Id { get; set; }

   public string NewPassword { get; set; } = null!;
}
